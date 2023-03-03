package com.mit.cycleonrent;

import android.annotation.SuppressLint;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.os.Parcelable;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.Toast;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * An example full-screen activity that shows and hides the system UI (i.e.
 * status bar and navigation/system bar) with user interaction.
 */
public class SplashActivity extends AppCompatActivity {


    ProgressDialog progressDialog;



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE); //will hide the title
        getSupportActionBar().hide(); // hide the title bar
        this.getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
                WindowManager.LayoutParams.FLAG_FULLSCREEN); //enable full screen

        setContentView(R.layout.activity_splash);

        ImageView backgroundImage = findViewById(R.id.logo_icon) ;
        Animation animation = AnimationUtils.loadAnimation(this, android.R.anim.slide_in_left);
        backgroundImage.startAnimation(animation);

        progressDialog = new ProgressDialog(this);




        //getSupportActionBar().hide();
        new Thread(new Runnable() {
            @Override
            public void run() {
                try {
                    Thread.sleep(3000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                //Intent intent = new Intent(getBaseContext(), MainActivity.class);praveen

                //Praveen for ref
                //SharedPreferences settings = getApplicationContext().getSharedPreferences("CYCLEONRENT_SETTINGS",0);
                //SharedPreferences.Editor editor = settings.edit();
                //editor.putInt("isMobileRegistered", YOUR_HOME_SCORE);
                //editor.apply();

                // Get from the SharedPreferences

                SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                        Context.MODE_PRIVATE);
                boolean isProfileCreated = settings.getBoolean("profileCreated",false);
                String registeredMobileNumber = settings.getString("registeredMobileNumber","");
                boolean isValidMobileN0 = false;
                if(registeredMobileNumber.length()==10 ){
                    isValidMobileN0 = true;
                }

                if (isProfileCreated && isValidMobileN0){

                    //praveen, activate navMap
                    Intent intent = new Intent(getBaseContext(), NavMapsActivity.class);
                    //Intent intent = new Intent(getBaseContext(), StartRideActivity.class);
                    //Bundle bundle = new Bundle();
                    //bundle.putStringArrayList("plateNumber", plateNumber);
                    //intent.putExtras(bundle);
                    startActivity(intent);
                    finish();

                }
                else if (isProfileCreated==false && isValidMobileN0){
                    Intent intent = new Intent(getBaseContext(), CreateProfileActivity.class);
                    startActivity(intent);
                    finish();

                }
                else{
                    Intent intent = new Intent(getBaseContext(), RegistrationActivity.class);
                    //Intent intent = new Intent(getBaseContext(), NavMapsActivity.class);//
                    //Intent intent = new Intent(getBaseContext(), OngoingRideActivity.class);
                    //Intent intent = new Intent(getBaseContext(), PaymentActivity.class);
                    startActivity(intent);
                    finish();
                }

            }
        }).start();
    }



}