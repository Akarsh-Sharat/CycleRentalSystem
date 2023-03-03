package com.mit.cycleonrent;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.google.firebase.auth.FirebaseAuth;

import static com.google.android.gms.common.internal.safeparcel.SafeParcelable.NULL;

public class ProfileActivity extends AppCompatActivity {

    String phoneNumber;
    TextView mobileNumber;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);

        // get saved phone number
        SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                Context.MODE_PRIVATE);
        String registeredMobileNumber = settings.getString("registeredMobileNumber","");
        boolean isValidMobileN0 = false;
        if(registeredMobileNumber.length()==12 && registeredMobileNumber.startsWith("91")){
            isValidMobileN0 = true;
        }


        phoneNumber = registeredMobileNumber;

        mobileNumber = findViewById(R.id.mobileNumber);
        mobileNumber.setText(phoneNumber);

        findViewById(R.id.buttonLogout).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                FirebaseAuth.getInstance().signOut();

                Intent intent = new Intent(ProfileActivity.this, RegistrationActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
                startActivity(intent);
            }
        });
    }
}
