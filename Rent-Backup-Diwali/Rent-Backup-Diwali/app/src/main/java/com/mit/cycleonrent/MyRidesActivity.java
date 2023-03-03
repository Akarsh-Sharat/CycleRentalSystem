package com.mit.cycleonrent;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

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
import com.google.android.material.navigation.NavigationView;
import com.google.firebase.firestore.FirebaseFirestore;
import com.mit.cycleonrent.Adapter.MyRideAdapter;
import com.shreyaspatil.EasyUpiPayment.EasyUpiPayment;
import com.shreyaspatil.EasyUpiPayment.listener.PaymentStatusListener;
import com.shreyaspatil.EasyUpiPayment.model.TransactionDetails;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;

public class MyRidesActivity extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener {

    RecyclerView recyclerView;
    RecyclerView.Adapter adapter;
    List<MyRide> myRides;
    String URL_Data="http://3.89.65.220/api/apiuserrideinfo";


    RequestQueue reqQue;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_myrides);

        Toolbar toolbar = findViewById(R.id.toolbar3_myrides);
        setSupportActionBar(toolbar);

        DrawerLayout drawer = findViewById(R.id.drawer_layout_myrides);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view_myrides);
        navigationView.setNavigationItemSelectedListener(this);

        SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                Context.MODE_PRIVATE);
        boolean isProfileCreated = settings.getBoolean("profileCreated",false);
        String registeredMobileNumber = settings.getString("registeredMobileNumber","");
        if(registeredMobileNumber.length()==10 ){
            URL_Data="http://3.89.65.220/api/apiuserrideinfo/"+registeredMobileNumber.substring(0,9);
            recyclerView=(RecyclerView)findViewById(R.id.recyleview);
            recyclerView.setHasFixedSize(true);
            recyclerView.setLayoutManager(new LinearLayoutManager(this));
            myRides=new ArrayList<>();

            loadurl(URL_Data);
        }
        else{
            Toast.makeText(MyRidesActivity.this, "Invalid Mobile Number, Please try again",Toast.LENGTH_SHORT).show();
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

    public void getvalue(JSONArray array) {

        for (int i = 0; i < array.length(); i++) {

            MyRide myRide = new MyRide();

            JSONObject json = null;
            try {
                json = array.getJSONObject(i);
                myRide.setDeviceId(json.getString("deviceId"));
                myRide.setRideDate(json.getString("rideDate"));
                myRide.setRideFromTime(json.getString("scanStartTime"));
                myRide.setRideToTime(json.getString("scanEndTime"));
                myRide.setRideDate(json.getString("rideDate"));
                myRide.setRideDuration(json.getString("rideDuration"));
                myRide.setRideFromLocation(json.getString("startPosition"));
                myRide.setRideToLocation(json.getString("endPosition"));
                myRide.setRideCost(json.getString("rideCost"));
                myRide.setRideRate(json.getString("rideRate"));


            } catch (JSONException e) {

                e.printStackTrace();
            }
            myRides.add(myRide);
        }

        adapter = new MyRideAdapter(myRides, this);

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


}