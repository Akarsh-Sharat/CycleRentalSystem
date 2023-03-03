package com.mit.cycleonrent;

import com.google.android.gms.maps.model.LatLng;
import com.google.firebase.Timestamp;
import com.google.firebase.firestore.Exclude;

import java.io.Serializable;
import java.util.Date;

// we have to implement our modal class
// with serializable so that we can pass
// our object class to new activity on
// our item click of recycler view.
public class StartRideModel implements Serializable {
    private LatLng position;
    // getter method for our id
    public String getId() {
        return id;
    }

    // setter method for our id
    public void setId(String id) {
        this.id = id;
    }

    // we are using exclude because
    // we are not saving our id
    @Exclude
    private String id;

    // variables for storing our data.
    private String mobileNumber,cycleNumber;
    private Timestamp startedAt,stoppedAt;
    private Date createdDate;

    double latitude;
    double longitude;

    public StartRideModel() {
        // empty constructor required for Firebase.
    }

    // Constructor for all variables.
    public StartRideModel(String mobileNumber, String cycleNumber,Timestamp startedAt,Timestamp stoppedAt,LatLng position) {
        this.mobileNumber = mobileNumber;
        this.cycleNumber = cycleNumber;
        this.startedAt = startedAt;
        this.stoppedAt = stoppedAt;
        this.position = position;
    }

    // getter methods for all variables.
    public String getMobileNumber() {
        return mobileNumber;
    }

    public void setMobileNumber(String mobileNumber) {
        this.mobileNumber = mobileNumber;
    }

    public String getCycleNumber() {
        return cycleNumber;
    }

    // setter method for all variables.
    public void setCycleNumber(String cycleNumber) {
        this.cycleNumber = cycleNumber;
    }

    public Timestamp getStartedAt() {
        return startedAt;
    }

    // setter method for all variables.
    public void setStartedAt(Timestamp startedAt) {
        this.startedAt = startedAt;
    }

    public Timestamp getStoppedAt() {
        return stoppedAt;
    }

    // setter method for all variables.
    public void setStoppedAt(Timestamp stoppedAt) {
        this.stoppedAt = stoppedAt;
    }

    public void setPosition(LatLng position) {
        this.position = position;
    }

    public LatLng getPosition() {
        return position;
    }

}

