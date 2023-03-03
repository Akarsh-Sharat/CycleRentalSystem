package com.mit.cycleonrent;

import java.util.HashMap;
import java.util.Map;

public class Post {


    private String id;

    private String plateNumber;
    private Map<String, String> location = new HashMap<>();
    private String isLocked;



    public String getId() {
        return id;
    }
    public String getPlateNumber() {
        return plateNumber;
    }
    public Map<String, String> getLocation() {
        return location;
    }
    public String isLocked() {
        return isLocked;
    }


    public void setId(String id) {
        this.id = id;
    }
    public void setPlateNumber(String plateNumber) {
        this.id = plateNumber;
    }
    public void setLocation(Map<String, String>  location) {
        this.location = location;
    }
    public void setIsLocked(String isLocked) {
        this.isLocked = isLocked;
    }
}
