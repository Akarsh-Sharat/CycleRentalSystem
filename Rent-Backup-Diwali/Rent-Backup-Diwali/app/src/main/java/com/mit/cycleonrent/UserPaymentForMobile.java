package com.mit.cycleonrent;

public class UserPaymentForMobile {

    private String transactionId;
    private String transactionDate;
    private String transactionAmount;
    private String transactionMode;
    public String getTransactionId() {
        return transactionId;
    }
    public String getTransactionDate() {
        return transactionDate;
    }
    public String getTransactionAmount() {
        return transactionAmount;
    }
    public String getTransactionMode() {
        return transactionMode;
    }

    public void setTransactionId(String transactionId) {
        this.transactionId = transactionId;
    }
    public void setTransactionDate(String transactionDate) {
        this.transactionDate = transactionDate;
    }
    public void setTransactionAmount(String transactionAmount) {
        this.transactionAmount = transactionAmount;
    }
    public void setTransactionMode(String transactionMode) {
        this.transactionMode = transactionMode;
    }

}
