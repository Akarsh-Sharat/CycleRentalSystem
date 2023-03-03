package com.mit.cycleonrent;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.material.navigation.NavigationView;
import com.google.firebase.firestore.DocumentReference;
import com.google.firebase.firestore.DocumentSnapshot;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.Source;

import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.mit.cycleonrent.Adapter.MyRideAdapter;
import com.mit.cycleonrent.Adapter.PaymentAdapter;
import com.shreyaspatil.EasyUpiPayment.EasyUpiPayment;
import com.shreyaspatil.EasyUpiPayment.listener.PaymentStatusListener;
import com.shreyaspatil.EasyUpiPayment.model.TransactionDetails;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;

public class PaymentActivity extends AppCompatActivity implements PaymentStatusListener,NavigationView.OnNavigationItemSelectedListener {

    private FirebaseFirestore firestore;
    private EditText amountEdt, upiEdt, nameEdt, descEdt;
    private TextView transactionDetailsTV;

    RecyclerView recyclerView;
    RecyclerView.Adapter adapter;
    List<UserPaymentForMobile> userPaymentForMobile;
    String URL_Data="http://3.89.65.220/api/apiPayment";
    RequestQueue reqQue;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_payment);

        Toolbar toolbar = findViewById(R.id.toolbar3_payment);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = findViewById(R.id.drawer_layout_payment);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view_payment);
        navigationView.setNavigationItemSelectedListener(this);


        // initializing all our variables.
        amountEdt = findViewById(R.id.idEdtAmount);
        upiEdt = findViewById(R.id.idEdtUpi);
        nameEdt = findViewById(R.id.idEdtName);
        descEdt = findViewById(R.id.idEdtDescription);
        Button makePaymentBtn = findViewById(R.id.idBtnMakePayment);
        transactionDetailsTV = findViewById(R.id.idTVTransactionDetails);

        // on below line we are getting date and then we are setting this date as transaction id.
        Date c = Calendar.getInstance().getTime();
        SimpleDateFormat df = new SimpleDateFormat("ddMMyyyyHHmmss", Locale.getDefault());
        String transcId = df.format(c);

        // on below line we are adding click listener for our payment button.
        makePaymentBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // on below line we are getting data from our edit text.
                String amount = amountEdt.getText().toString();
                String upi = upiEdt.getText().toString();
                String name = nameEdt.getText().toString();
                String desc = descEdt.getText().toString();
                // on below line we are validating our text field.
                if (TextUtils.isEmpty(amount) && TextUtils.isEmpty(upi) && TextUtils.isEmpty(name) && TextUtils.isEmpty(desc)) {
                    Toast.makeText(PaymentActivity.this, "Please enter all the details..", Toast.LENGTH_SHORT).show();
                } else {
                    // if the edit text is not empty then
                    // we are calling method to make payment.
                    makePayment(amount, upi, name, desc, transcId);
                }
            }
        });


        SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                Context.MODE_PRIVATE);
        boolean isProfileCreated = settings.getBoolean("profileCreated",false);
        String registeredMobileNumber = settings.getString("registeredMobileNumber","");
        if(registeredMobileNumber.length()==10 ){
            URL_Data="http://3.89.65.220/api/ApiUserPayment/"+registeredMobileNumber.substring(0,9);
            recyclerView=(RecyclerView)findViewById(R.id.recyleview);
            recyclerView.setHasFixedSize(true);
            recyclerView.setLayoutManager(new LinearLayoutManager(this));
            loadurl(URL_Data);
        }
        else{
            Toast.makeText(PaymentActivity.this, "Invalid Mobile Number, Please try again",Toast.LENGTH_SHORT).show();
            return;
        }

    }
    public void loadurl(String url) {
        JsonArrayRequest stringRequest=new JsonArrayRequest(url, new Response.Listener<JSONArray>() {

            @Override
            public void onResponse(JSONArray response) {
                getvalue(response);

            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {

            }
        });

        reqQue = Volley.newRequestQueue(this);

        reqQue.add(stringRequest);
    }

    public void getvalue(JSONArray objPayment) {

        JSONArray array = null;
        PaymentModel userPaymentSummary = new PaymentModel();
        userPaymentForMobile = new ArrayList<>();
        try {
            JSONObject summary = objPayment.getJSONObject(0);
            String totalPaidAmt = summary.getString("totalPaidAmount");
            String totalBalAmt = summary.getString("totalBalanceAmount");
            String totalRideAmt = summary.getString("totalRideAmount");

            ((TextView)findViewById(R.id.TotalPayment)).setText(totalPaidAmt);
           ((TextView)findViewById(R.id.TotalBalance)).setText(totalBalAmt);
           ((TextView)findViewById(R.id.TotalRide)).setText(totalRideAmt);
            array = objPayment.getJSONObject(0).getJSONArray("userPaymentForMobile");
        } catch (JSONException e) {
            e.printStackTrace();
        }
        for (int i = 0; i < array.length(); i++) {

            UserPaymentForMobile payment = new UserPaymentForMobile();

            JSONObject json = null;
            try {
                json = array.getJSONObject(i);
                payment.setTransactionId(json.getString("transactionId"));
                payment.setTransactionDate(json.getString("paymentDate") + " " + json.getString("paymentTime"));
                payment.setTransactionAmount(json.getString("amount"));
                payment.setTransactionMode(json.getString("paymentModeName"));


            } catch (JSONException e) {

                e.printStackTrace();
            }
            userPaymentForMobile.add(payment);
        }


        adapter = new PaymentAdapter(userPaymentForMobile, this);

        recyclerView.setAdapter(adapter);
    }


    @Override
    public void onBackPressed() {
        DrawerLayout drawer = findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        //getMenuInflater().inflate(R.menu.nav_maps, menu);
        //return true;

        final MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.nav_maps, menu);

        return true;
    }


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();
        //clearBackStack();
        if (id == R.id.find_cycle) {
            Intent intent = new Intent(getBaseContext(), NavMapsActivity.class);
            startActivity(intent);
            finish();
        } else if (id == R.id.nav_startRide) {

            Intent intent = new Intent(getBaseContext(), StartRideActivity.class);
            startActivity(intent);
            finish();

        } else if (id == R.id.nav_myRides) {
            Intent intent = new Intent(getBaseContext(), StartRideActivity.class);
            startActivity(intent);
            finish();
        } else if (id == R.id.nav_payments) {

            Intent intent = new Intent(getBaseContext(), PaymentActivity.class);
            startActivity(intent);
            finish();
        }

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

    private void makePayment(String amount, String upi, String name, String desc, String transactionId) {
        // on below line we are calling an easy payment method and passing
        // all parameters to it such as upi id,name, description and others.
        if(!amount.contains(".")){
            amount = amount+".0";
        }
        final EasyUpiPayment easyUpiPayment = new EasyUpiPayment.Builder()
                .with(this)
                // on below line we are adding upi id.
                .setPayeeVpa(upi)
                // on below line we are setting name to which we are making payment.
                .setPayeeName(name)
                // on below line we are passing transaction id.
                .setTransactionId(transactionId)
                // on below line we are passing transaction ref id.
                .setTransactionRefId(transactionId)
                // on below line we are adding description to payment.
                .setDescription(desc)
                // on below line we are passing amount which is being paid.
                .setAmount(amount)
                // on below line we are calling a build method to build this ui.
                .build();
        // on below line we are calling a start
        // payment method to start a payment.
        easyUpiPayment.startPayment();
        // on below line we are calling a set payment
        // status listener method to call other payment methods.
        easyUpiPayment.setPaymentStatusListener(this);
    }

    @Override
    public void onTransactionCompleted(TransactionDetails transactionDetails) {
        // on below line we are getting details about transaction when completed.
        String transcDetails = transactionDetails.getStatus().toString() + "\n" + "Transaction ID : " + transactionDetails.getTransactionId();
        transactionDetailsTV.setVisibility(View.VISIBLE);
        // on below line we are setting details to our text view.
        transactionDetailsTV.setText(transcDetails);
    }

    @Override
    public void onTransactionSuccess() {
        // this method is called when transaction is successful and we are displaying a toast message.
        Toast.makeText(this, "Transaction successfully completed..", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onTransactionSubmitted() {
        // this method is called when transaction is done
        // but it may be successful or failure.
        Log.e("TAG", "TRANSACTION SUBMIT");
    }

    @Override
    public void onTransactionFailed() {
        // this method is called when transaction is failure.
        Toast.makeText(this, "Failed to complete transaction", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onTransactionCancelled() {
        // this method is called when transaction is cancelled.
        Toast.makeText(this, "Transaction cancelled..", Toast.LENGTH_SHORT).show();
    }

    @Override
    public void onAppNotFound() {
        // this method is called when the users device is not having any app installed for making payment.
        Toast.makeText(this, "No app found for making transaction..", Toast.LENGTH_SHORT).show();
    }

}