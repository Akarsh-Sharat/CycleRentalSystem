package com.mit.cycleonrent;

import java.util.List;

public class PaymentModel {

    private String totalRideAmount;
    private String totalPaidAmount;
    private String totalBalanceAmount;

    private  List<UserPaymentForMobile> userPaymentForMobile;

    public String getTotalRideAmount() {
        return totalRideAmount;
    }
    public String getTotalPaidAmount() {
            return totalPaidAmount;
        }
    public String getTotalBalanceAmount() {
        return totalBalanceAmount;
    }
    public List<UserPaymentForMobile> getUserPaymentForMobile() {
        return userPaymentForMobile;
    }


    public void setTotalRideAmount(String totalRideAmount) {
        this.totalRideAmount = totalRideAmount;
    }
    public void setTotalPaidAmount(String totalPaidAmount) {
        this.totalPaidAmount = totalPaidAmount;
    }
    public void setTotalBalanceAmount(String totalBalanceAmount) {
        this.totalBalanceAmount = totalBalanceAmount;
    }

    public void setUserPaymentForMobile(List<UserPaymentForMobile> userPaymentForMobile) {
        this.userPaymentForMobile = userPaymentForMobile;
    }

    }

