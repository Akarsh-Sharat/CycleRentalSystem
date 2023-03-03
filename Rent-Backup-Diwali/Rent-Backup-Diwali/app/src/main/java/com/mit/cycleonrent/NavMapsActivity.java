package com.mit.cycleonrent;

import android.Manifest;
import android.app.ListActivity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.ColorSpace;
import android.location.Location;
import android.location.LocationListener;
import android.os.Build;
import android.os.Bundle;


import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.appcompat.widget.Toolbar;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;
import androidx.fragment.app.FragmentManager;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.google.android.gms.common.GooglePlayServicesNotAvailableException;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapsInitializer;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.navigation.NavigationView;
import com.google.maps.android.clustering.ClusterManager;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

public class NavMapsActivity extends AppCompatActivity implements OnMapReadyCallback,NavigationView.OnNavigationItemSelectedListener {

    private GoogleMap mMap;
    private static String TAG = "MESSAGE";
    private FusedLocationProviderClient mFusedLocationProviderClient;
    private Boolean permissionGranted = false;
    private static float ZOOM = 15f;
    private Boolean focusOnUser = true;


    public String[] plateNumber;
    public String[] latitudes;
    public String[] longitude;
    public String[] title;
    public String[] distanceToUser;

    private ClusterManager mClusterManager;
    private ClusterRenderer mClusterRenderer;
    private ArrayList<ClusterMarker> mClusterMarkers;
    public double lat;
    public double lon;
    public String carId = "-1";
    public String buttonId = "-1";
    private NavigationView nvDrawer;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_nav_maps);


        MapsInitializer.initialize(this);

        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar2);
        //setSupportActionBar(toolbar);

        DrawerLayout drawer =  findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);


        mClusterMarkers = new ArrayList<ClusterMarker>();


        refresh_list();


        //latitudes = new String[]{"18.60073698939555","18.600790"};
        //longitude = new String[]{"73.79438279363727","73.793454"};



        checkForContactsPermissions();

        FloatingActionButton myFab = (FloatingActionButton) findViewById(R.id.startRideButton);
        myFab.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                startRide();
            }
        });


    }

/////////////////////////////////////////////////////////

    private void refresh_list(){
        StringRequest stringRequest = new StringRequest(Request.Method.GET, "http://3.89.65.220/api/apideviceshadow", new Response.Listener<String>() {
            @Override
            public void onResponse(String response) {
                //progressDialog.dismiss();
                try{
                    //progressDialog.hide();
                    JSONArray jsonArray = new JSONArray(response);

                    plateNumber = new String[jsonArray.length()];
                    title = new String[jsonArray.length()];
                    distanceToUser = new String[jsonArray.length()];
                    longitude = new String[jsonArray.length()];
                    latitudes = new String[jsonArray.length()];


                    for (int i = 0; i<jsonArray.length(); i++){
                        JSONObject o = jsonArray.getJSONObject(i);
                        JSONObject oLocation = new JSONObject(o.getString("location"));
                        JSONObject info = new JSONObject(o.getString("info"));



                        latitudes[i]=oLocation.getString("lat");
                        longitude[i]=oLocation.getString("lon");
                        title[i]=o.getString("deviceId");


                        distanceToUser[i] = String.valueOf(GetDistance(lat, lon, Double.parseDouble(latitudes[i]), Double.parseDouble(longitude[i])));

                        String allInfo="Distance : " + distanceToUser[i]+" Meter , ";
                        for(Iterator<String> iter = info.keys(); iter.hasNext();) {
                            String key = iter.next();
                            allInfo =allInfo + key + " : " + info.getString(key) + " , ";
                        }
                        plateNumber[i] = allInfo;


                    }
                    addCarMarkers();



                }catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                //progressDialog.hide();
                Toast.makeText(NavMapsActivity.this, "Failed",Toast.LENGTH_SHORT).show();
            }
        }){
            protected Map<String , String> getParams() throws AuthFailureError {
                Map<String , String> params = new HashMap<>();
                params.put("name", "kl");
                return params;
            }
        };
        RequestHandler.getInstance(NavMapsActivity.this).addToRequestQueue(stringRequest);

    }

    private void startRide(){
        Intent intent = new Intent(getBaseContext(), StartRideActivity.class);
        startActivity(intent);
        finish();
    }
    private void StartMap() {

        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);
    }

    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;

        if(permissionGranted)
        {
            GetCurrentLocation();
        }

        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED
                && ActivityCompat.checkSelfPermission
                (this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED)
        {
            return;
        }
        mMap.setMyLocationEnabled(true);


    }

    private void GetCurrentLocation()
    {

        mFusedLocationProviderClient = LocationServices.getFusedLocationProviderClient(this);

        try{
            if(permissionGranted)
            {
                final Task location = mFusedLocationProviderClient.getLastLocation();
                location.addOnCompleteListener(new OnCompleteListener() {
                    @Override
                    public void onComplete(@NonNull Task task) {
                        if(task.isSuccessful())
                        {
                            Location currentLocation = (Location) task.getResult();
                            if(currentLocation ==null){
                                return;
                            }
                            lat = currentLocation.getLatitude();
                            lon = currentLocation.getLongitude();
                            LatLng ll = new LatLng(lat,lon);
                            if(focusOnUser && carId.equals("-1")) moveCameraView(ll,ZOOM);
                            else{
                                ll = new LatLng(Double.parseDouble(latitudes[Integer.parseInt(carId)]),Double.parseDouble(longitude[Integer.parseInt(carId)]));
                                moveCameraView(ll,ZOOM);

                            }
                        }
                        else
                        {
                            Toast.makeText(NavMapsActivity.this, "Unable to detect current location", Toast.LENGTH_SHORT).show();

                        }
                    }
                });
            }
        }catch (SecurityException e)
        {
            //Log.e(TAG, "GetCurrentLocation: Security Exception: " + e.getMessage());
        }
    }


    /**
     Method to move map to certain point (move camera)
     */

    private void moveCameraView(LatLng latLng, float zoom)
    {
        mMap.moveCamera(CameraUpdateFactory.newLatLngZoom(latLng, zoom));
    }



    public void addCarMarkers()
    {
        try {
            if (mMap != null) {
                if (mClusterManager == null) {
                    mClusterManager = new ClusterManager<ClusterMarker>(NavMapsActivity.this, mMap);
                }
                if (mClusterRenderer == null) {
                    mClusterRenderer = new ClusterRenderer(this, mMap, mClusterManager);
                    mClusterManager.setRenderer(mClusterRenderer);
                }
                for (int x = 0; x < latitudes.length ; x++) {
                    String snippet = plateNumber[x];
                    int avatar = R.drawable.ic_baseline_directions_bike_24;
                     ClusterMarker newClusterMarker = new ClusterMarker(
                            new LatLng(Double.parseDouble(latitudes[x]), Double.parseDouble(longitude[x])),
                             title[x],
                            snippet,
                            avatar
                    );

                    mClusterManager.addItem(newClusterMarker);
                    mClusterMarkers.add(newClusterMarker);

                }
                mClusterManager.cluster();

            }
        }catch (NullPointerException e){
            //Log.e(TAG, "addMapMarkers: NullPointerException: " + e.getMessage() );
        }
    }



    /**
     Method to calculate distance between car and user
     Using Haversine method
     */
    private static double GetDistance(double lat1, double lon1, double lat2, double lon2) {
        if ((lat1 == lat2) && (lon1 == lon2)) {
            return 0;
        }
        else {
            double theta = lon1 - lon2;
            double dist = Math.sin(Math.toRadians(lat1)) * Math.sin(Math.toRadians(lat2)) + Math.cos(Math.toRadians(lat1)) * Math.cos(Math.toRadians(lat2)) * Math.cos(Math.toRadians(theta));
            dist = Math.acos(dist);
            dist = Math.toDegrees(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            dist = Math.ceil(dist);//in meter
            //dist = dist /1000;
            return (dist);
        }
    }






    /**
     Permissions
     */
    private static String[] PERMISSIONS_CONTACT = {Manifest.permission.ACCESS_FINE_LOCATION,
            Manifest.permission.ACCESS_COARSE_LOCATION };
    private static final int REQUEST_CONTACTS = 1;

    private void checkForContactsPermissions() {
        permissionGranted = false;
        // Check if all required contact permissions have been granted.
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED
                || ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {

            // Permission not granted.
            //Log.i(TAG, "Location permissions have NOT been granted. Requesting permissions.");
            ActivityCompat.requestPermissions(NavMapsActivity.this, PERMISSIONS_CONTACT, REQUEST_CONTACTS);
            ActivityCompat.requestPermissions(NavMapsActivity.this, PERMISSIONS_CONTACT, REQUEST_CONTACTS);
        } else {
            // Permissions have been granted.
            //Log.i(TAG, "Location permissions have already been granted. MAP is ready.");
            permissionGranted = true;
            StartMap();
        }

    }

    /**
     * Callback received when a permissions request has been completed.
     */
    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {
        if (requestCode == REQUEST_CONTACTS) {
            //Log.i(TAG, "Received response for contact permissions request.");

            if (verifyPermissions(grantResults)) {
                //Log.i(TAG, "GRANTED");
                permissionGranted = true;
                StartMap();
            } else {
                //Log.i(TAG, "DENIED");
            }
        } else {
            super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
    private  boolean verifyPermissions(int[] grantResults) {
        if(grantResults.length < 1){
            return false;
        }
        for (int result : grantResults) {
            if (result != PackageManager.PERMISSION_GRANTED) {
                return false;
            }
        }
        return true;
    }


    /////////////////////////////////////////////////////////

    @Override
    public void onBackPressed() {
        DrawerLayout drawer =  findViewById(R.id.drawer_layout);
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




}
