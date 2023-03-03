package com.mit.cycleonrent;

import android.Manifest;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.location.Location;
import android.os.Bundle;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import com.google.android.gms.tasks.Task;
import com.google.android.gms.vision.CameraSource;
import com.google.android.gms.vision.Detector;
import com.google.android.gms.vision.barcode.Barcode;
import com.google.android.gms.vision.barcode.BarcodeDetector;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.navigation.NavigationView;
import com.google.android.material.snackbar.Snackbar;
import com.google.firebase.Timestamp;
import com.google.firebase.firestore.FirebaseFirestore;

import androidx.annotation.NonNull;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.cardview.widget.CardView;
import androidx.core.app.ActivityCompat;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;

import android.util.SparseArray;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.View;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

import static com.google.android.gms.common.internal.safeparcel.SafeParcelable.NULL;

public class StartRideActivity extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener {

    SurfaceView surfaceView;
    TextView textViewBarCodeValue;
    private BarcodeDetector barcodeDetector;
    private CameraSource cameraSource;
    private static final int REQUEST_CAMERA_PERMISSION = 201;
    String intentData = "";
    private FirebaseFirestore firestore;
    private FusedLocationProviderClient mFusedLocationProviderClient;

    double latStart = 0;
    double lonStart = 0;
    Button buttonStartRide;
    private ProgressBar loadingPB;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_start_ride);

        Toolbar toolbar = findViewById(R.id.toolbar3);
        setSupportActionBar(toolbar);


        DrawerLayout drawer = findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        initComponents();

        loadingPB = findViewById(R.id.progressbar);
        //sendJsonPostRequest("988110","RENT500001","startP");
        //GetDeviceInfo("RENT500001");
        buttonStartRide = findViewById(R.id.buttonStartRide);
        buttonStartRide.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                        Context.MODE_PRIVATE);
                boolean isProfileCreated = settings.getBoolean("profileCreated",false);
                String registeredMobileNumber = settings.getString("registeredMobileNumber","");
                if(registeredMobileNumber.length()==10 ){
                    String URL_Data="http://3.89.65.220/api/PostUserRideInfo";
                    String cycleNumber = ((TextView)findViewById(R.id.cycleNumberHiden)).getText().toString();
                    String startPosition = ((TextView)findViewById(R.id.startPositionHiden)).getText().toString();

                    sendJsonPostRequest(registeredMobileNumber.substring(0,9),cycleNumber,startPosition);

                }
                else{
                    Toast.makeText(StartRideActivity.this, "Invalid Mobile Number, Please try again",Toast.LENGTH_SHORT).show();
                    return;
                }

            }
        });

    }

    private void sendJsonPostRequest(String mobileNumberShort,String cycleNumber,String startPosition){

        try {
            loadingPB.setVisibility(View.VISIBLE);
            // Make new json object and put params in it
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("UserAccountId", mobileNumberShort);
            jsonParams.put("DeviceId", cycleNumber);
            jsonParams.put("StartPosition", startPosition);

            // Building a request
            JsonObjectRequest request = new JsonObjectRequest(
                    Request.Method.POST,
                    // Using a variable for the domain is great for testing
                    "http://3.89.65.220/api/ApiUserRideInfo",
                    jsonParams,
                    new Response.Listener<JSONObject>() {
                        @Override
                        public void onResponse(JSONObject response) {

                            loadingPB.setVisibility(View.GONE);
                            Toast.makeText(StartRideActivity.this, "Ride Started.....", Toast.LENGTH_SHORT).show();
                            ((Button)findViewById(R.id.buttonStartRide)).setBackgroundColor(Color.RED);
                            ((Button)findViewById(R.id.buttonStartRide)).setText("STOP RIDE");
                            ((SurfaceView)findViewById(R.id.surfaceView)).setVisibility(View.GONE);
                            ((Button)findViewById(R.id.buttonStartRide)).setEnabled(false);
                        }
                    },

                    new Response.ErrorListener(){
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            loadingPB.setVisibility(View.GONE);
                            // Handle the error
                            //Toast.makeText(StartRideActivity.this, "Error While creating Profile" + error.getMessage(), Toast.LENGTH_SHORT).show();
                            Toast.makeText(StartRideActivity.this, "Ride Started.....", Toast.LENGTH_SHORT).show();
                            ((Button)findViewById(R.id.buttonStartRide)).setBackgroundColor(Color.RED);
                            ((Button)findViewById(R.id.buttonStartRide)).setText("STOP RIDE");
                            ((SurfaceView)findViewById(R.id.surfaceView)).setVisibility(View.GONE);

                        }
                    });


            Volley.newRequestQueue(getApplicationContext()).
                    add(request);

        } catch(JSONException ex){
            loadingPB.setVisibility(View.GONE);
            String bb = ex.getMessage();
            // Catch if something went wrong with the params
        }

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
            Intent intent = new Intent(getBaseContext(), MyRidesActivity.class);
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

    private void initComponents() {
        textViewBarCodeValue = findViewById(R.id.txtBarcodeValue);
        surfaceView = findViewById(R.id.surfaceView);
    }

    private void initialiseDetectorsAndSources() {
        Toast.makeText(getApplicationContext(), "Barcode scanner started", Toast.LENGTH_SHORT).show();
        barcodeDetector = new BarcodeDetector.Builder(this)
                .setBarcodeFormats(Barcode.ALL_FORMATS)
                .build();

        cameraSource = new CameraSource.Builder(this, barcodeDetector)
                .setRequestedPreviewSize(1920, 1080)
                .setAutoFocusEnabled(true) //you should add this feature
                .build();

        surfaceView.getHolder().addCallback(new SurfaceHolder.Callback() {
            @Override
            public void surfaceCreated(SurfaceHolder holder) {
                openCamera();
            }
            @Override
            public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {
            }
            @Override
            public void surfaceDestroyed(SurfaceHolder holder) {
                cameraSource.stop();
            }
        });

        barcodeDetector.setProcessor(new Detector.Processor<Barcode>() {
            @Override
            public void release() {
                Toast.makeText(getApplicationContext(), "To prevent memory leaks barcode scanner has been stopped", Toast.LENGTH_SHORT).show();
            }

            @Override
            public void receiveDetections(Detector.Detections<Barcode> detections) {
                final SparseArray<Barcode> barCode = detections.getDetectedItems();
                if (barCode.size() > 0) {
                    setBarCode(barCode);
                }
            }
        });
    }

    private void openCamera(){
        try {
            if (ActivityCompat.checkSelfPermission(StartRideActivity.this, Manifest.permission.CAMERA) == PackageManager.PERMISSION_GRANTED) {
                cameraSource.start(surfaceView.getHolder());
            } else {
                ActivityCompat.requestPermissions(StartRideActivity.this, new
                        String[]{Manifest.permission.CAMERA}, REQUEST_CAMERA_PERMISSION);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void setBarCode(final SparseArray<Barcode> barCode){
        textViewBarCodeValue.post(new Runnable() {
            @Override
            public void run() {
                intentData = barCode.valueAt(0).displayValue;
                Toast.makeText(getApplicationContext(), "Scan Code = " + intentData, Toast.LENGTH_SHORT).show();

                //textViewBarCodeValue.setText(intentData);
                //copyToClipBoard(intentData);

                if(intentData.startsWith(("RENT"))){
                    loadingPB.setVisibility(View.GONE);
                    cameraSource.stop();
                    GetDeviceInfo(intentData);
                }
            }
        });
    }
    /*private void openCamera() {
        try {
            if (ActivityCompat.checkSelfPermission(StartRideActivity.this, Manifest.permission.CAMERA) == PackageManager.PERMISSION_GRANTED) {
                cameraSource.start(surfaceView.getHolder());
            } else {
                ActivityCompat.requestPermissions(StartRideActivity.this, new
                        String[]{Manifest.permission.CAMERA}, REQUEST_CAMERA_PERMISSION);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void setBarCode(final SparseArray<Barcode> barCode) {
        textViewBarCodeValue.post(new Runnable() {
            @Override
            public void run() {
                intentData = barCode.valueAt(0).displayValue;
                textViewBarCodeValue.setText(intentData);
                cameraSource.stop();
                Toast.makeText(getApplicationContext(), "Cycle No. = " + intentData, Toast.LENGTH_SHORT).show();

                //saveToDB(intentData);
                GetDeviceInfo(intentData);
                //copyToClipBoard(intentData);
            }
        });
    }*/


    @Override
    protected void onPause() {
        super.onPause();
        if(cameraSource != null) {
            cameraSource.release();
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        initialiseDetectorsAndSources();//Praveen scanner
    }

    private void GetDeviceInfo(String cycleNumber) {
        loadingPB.setVisibility(View.VISIBLE);
        ((TextView)findViewById(R.id.cycleNumberHiden)).setText(cycleNumber);

        if(cycleNumber.startsWith(("RENT")) && cycleNumber.length() == 10){

        }
        else{
            cycleNumber = "RENT500001";
        }
        // get saved phone number
        SharedPreferences settings = getApplicationContext().getSharedPreferences("RENT_USER_PREF",
                Context.MODE_PRIVATE);
        String registeredMobileNumber = settings.getString("registeredMobileNumber", "");
        String mobileNumber = "";
        boolean isValidMobileN0 = false;
        if (registeredMobileNumber.length() == 10) {
            mobileNumber = registeredMobileNumber;
        }
        else {
            Toast.makeText(StartRideActivity.this, "Invalid Mobile Number. Please scan again", Toast.LENGTH_SHORT).show();
        }
        /////////////////////DB //////////////////////////

        // On service layer, we have to send int to save develoment time so mergin mobile number and deviceid

        String idtosend = mobileNumber.substring(0,4)+cycleNumber.substring(cycleNumber.length()-5,cycleNumber.length());
        StringRequest stringRequest = new StringRequest(Request.Method.GET, "http://3.89.65.220/api/apideviceshadow/"+idtosend, new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                //progressDialog.dismiss();
                try{
                    loadingPB.setVisibility(View.GONE);

                    JSONObject jsonObj = new JSONObject(response);
                    ((TextView)findViewById(R.id.DeviceId)).setText(jsonObj.getString("deviceId"));
                    ((TextView)findViewById(R.id.DeviceModel)).setText(jsonObj.getString("deviceModelName"));
                    ((TextView)findViewById(R.id.Rate)).setText(jsonObj.getString("rate"));

                    JSONObject info = new JSONObject(jsonObj.getString("info"));
                    String allInfo="";

                        for(Iterator<String> iter = info.keys(); iter.hasNext();) {
                            String key = iter.next();
                            allInfo =allInfo + key + " : " + info.getString(key) + " \n ";
                        }

                    ((TextView)findViewById(R.id.Info)).setText(allInfo);
                    ((TextView)findViewById(R.id.startPositionHiden)).setText(jsonObj.getString("location"));

                    JSONObject locat = new JSONObject(jsonObj.getString("location"));
                    String allLocat="";

                    for(Iterator<String> iter = locat.keys(); iter.hasNext();) {
                        String key = iter.next();
                        allLocat =allLocat + key + " : " + locat.getString(key) + " \n ";
                    }

                    ((TextView)findViewById(R.id.Location)).setText(allLocat);
                    ((CardView)findViewById(R.id.card)).setVisibility(View.VISIBLE);

                    buttonStartRide.setVisibility(View.VISIBLE);
                }catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                loadingPB.setVisibility(View.GONE);
                buttonStartRide.setVisibility(View.VISIBLE);
                Toast.makeText(StartRideActivity.this, "Failed",Toast.LENGTH_SHORT).show();
            }
        }){
            protected Map<String , String> getParams() throws AuthFailureError {
                Map<String , String> params = new HashMap<>();
                params.put("name", "kl");
                return params;
            }
        };
        RequestHandler.getInstance(StartRideActivity.this).addToRequestQueue(stringRequest);


    }


    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if(requestCode == REQUEST_CAMERA_PERMISSION && grantResults.length>0){
            if (grantResults[0] == PackageManager.PERMISSION_DENIED)
                finish();
            else
                openCamera();
        }else
            finish();
    }


}