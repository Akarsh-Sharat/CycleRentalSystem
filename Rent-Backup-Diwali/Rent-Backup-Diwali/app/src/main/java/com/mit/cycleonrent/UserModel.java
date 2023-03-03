package com.mit.cycleonrent;

import com.google.firebase.firestore.Exclude;

import java.io.Serializable;
import java.util.Date;

// we have to implement our modal class
// with serializable so that we can pass
// our object class to new activity on
// our item click of recycler view.
public class UserModel implements Serializable {

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
    private String mobileNumber,email,firstName, lastName;
    private Date createdDate;
    public UserModel() {
        // empty constructor required for Firebase.
    }

    // Constructor for all variables.
    public UserModel(String mobileNumber, String email, String firstName, String lastName) {
        this.mobileNumber = mobileNumber;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
    }

    // getter methods for all variables.
    public String getMobileNumber() {
        return mobileNumber;
    }

    public void setMobileNumber(String mobileNumber) {
        this.mobileNumber = mobileNumber;
    }

    public String getEmail() {
        return email;
    }

    // setter method for all variables.
    public void setEmail(String email) {
        this.email = email;
    }

    public String getFirstName() {
        return firstName;
    }
    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }
    public void getLastName(String lastName) {
        this.lastName = lastName;
    }
}

