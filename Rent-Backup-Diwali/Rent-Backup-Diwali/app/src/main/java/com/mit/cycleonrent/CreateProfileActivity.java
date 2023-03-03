package com.mit.cycleonrent;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.firestore.FirebaseFirestore;


import org.json.JSONException;
import org.json.JSONObject;

import java.util.Calendar;
import java.util.HashMap;
import java.util.Map;

import static com.google.android.gms.common.internal.safeparcel.SafeParcelable.NULL;

public class CreateProfileActivity extends AppCompatActivity {

    String phoneNumber;
    TextView textViewMobile;
    EditText txtFirstName;
    EditText txtLastName;
    EditText txtEmail;
    Button buttonSignup;
    private ProgressBar loadingPB;
    private FirebaseFirestore firestore;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_profile);

        // get saved phone number

        SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                Context.MODE_PRIVATE);
        String registeredMobileNumber = settings.getString("registeredMobileNumber","");
        boolean isValidMobileN0 = false;
        if(registeredMobileNumber.length()==10){
            phoneNumber = registeredMobileNumber;
        }
        else{
            //send back for re registration from start as unknown mobile number
            Intent intent = new Intent(getBaseContext(), RegistrationActivity.class);
            startActivity(intent);
            finish();
        }

        textViewMobile = findViewById(R.id.textViewMobile);
        textViewMobile.setText(phoneNumber);
        buttonSignup = findViewById(R.id.buttonSignup);

        txtFirstName = findViewById(R.id.editTextFirstName);
        txtLastName = findViewById(R.id.editTextLastName);
        txtEmail = findViewById(R.id.editTextEmail);
        loadingPB = findViewById(R.id.progressbar);
/*

 praveen - logoutcode
        findViewById(R.id.buttonLogout).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                FirebaseAuth.getInstance().signOut();

                Intent intent = new Intent(CreateProfileActivity.this, MainActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
                startActivity(intent);
            }
        });
        */


        buttonSignup.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                String fName = txtFirstName.getText().toString().trim();
                String lName = txtLastName.getText().toString().trim();
                String email = txtEmail.getText().toString().trim();

                if (fName.isEmpty() || fName.length() < 2) {

                    txtFirstName.setError("Enter First Name...");
                    txtFirstName.requestFocus();
                    return;
                }
                if (lName.isEmpty() || lName.length() < 2) {

                    txtLastName.setError("Enter Last Name...");
                    txtLastName.requestFocus();
                    return;
                }
                if (email.isEmpty() || email.length() < 2 || email.contains("@") == false || email.contains(".") == false) {

                    txtEmail.setError("Enter Valid Email...");
                    txtEmail.requestFocus();
                    return;
                }
                if (email.indexOf("@") > email.lastIndexOf(".")) {

                    txtEmail.setError("Enter valid Email...");
                    txtEmail.requestFocus();
                    return;
                }
                //updateCourses(fName,lName,email);
                //postDataUsingVolley(fName,lName,email);
                sendJsonPostRequest(fName,lName,email);
                // save phone number
                SharedPreferences prefs = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                        Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = prefs.edit();
                editor.putBoolean("profileCreated", true);
                editor.apply();

                //Intent intent = new Intent(getBaseContext(), NavMapsActivity.class);//praveen
                //startActivity(intent);
            }
        });
    };
    private void sendJsonPostRequest(String fName,String lName,String email){

        try {
            loadingPB.setVisibility(View.VISIBLE);
            // Make new json object and put params in it
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("email", email);
            jsonParams.put("firstName", fName);
            jsonParams.put("lastName", lName);
            jsonParams.put("mobileNumber", phoneNumber);
            jsonParams.put("status", "A");
            jsonParams.put("createdDate", "2022-11-05T18:02:38.191Z");
            jsonParams.put("usercategoryId", 0);


            // Building a request
            JsonObjectRequest request = new JsonObjectRequest(
                    Request.Method.POST,
                    // Using a variable for the domain is great for testing
                    "http://3.89.65.220/api/apiuseraccount",
                    jsonParams,
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {

                            loadingPB.setVisibility(View.GONE);
                            //txtFirstName.setText("");
                            //txtLastName.setText("");
                            //txtEmail.setText("");
                            //textViewMobile.setText("");
                            Toast.makeText(CreateProfileActivity.this, "Profile has been updated..", Toast.LENGTH_SHORT).show();

                            Intent intent = new Intent(getBaseContext(), NavMapsActivity.class);//praveen
                            startActivity(intent);
                            //String bb = "response.getString()";
                            // Handle the response

                        }
                    },

                    new Response.ErrorListener(){
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            loadingPB.setVisibility(View.GONE);
                            // Handle the error
                            Toast.makeText(CreateProfileActivity.this, "Error While creating Profile" + error.getMessage(), Toast.LENGTH_SHORT).show();

                        }
                    });

            /*

              For the sake of the example I've called newRequestQueue(getApplicationContext()) here
              but the recommended way is to create a singleton that will handle this.

              Read more at : https://developer.android.com/training/volley/requestqueue

              Category -> Use a singleton pattern

            */
            Volley.newRequestQueue(getApplicationContext()).
                    add(request);

        } catch(JSONException ex){
            loadingPB.setVisibility(View.GONE);
            String bb = ex.getMessage();
            // Catch if something went wrong with the params
        }

    }

}
