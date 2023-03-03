package com.mit.cycleonrent.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.recyclerview.widget.RecyclerView;

import com.mit.cycleonrent.MyRide;
import com.mit.cycleonrent.PaymentModel;
import com.mit.cycleonrent.R;
import com.mit.cycleonrent.UserPaymentForMobile;

import java.util.List;

public class PaymentAdapter extends RecyclerView.Adapter<PaymentAdapter.ViewHolder> {
    Context context;
    List<UserPaymentForMobile> data;
    TextView TransactionDate;
    TextView TransactionId;
    TextView TransactionAmount;
    TextView TransactionMode;


    public PaymentAdapter(List<UserPaymentForMobile> data, Context context)
    {
        this.context=context;
        this.data=data;

    }
    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
       View view=LayoutInflater.from(parent.getContext()).inflate(R.layout.content_payments_card,parent,false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        final UserPaymentForMobile geter1 =  data.get(position);
        String transactionId,transactionDate, transactionAmount, transactionMode;

        transactionId=geter1.getTransactionId();
        transactionDate=geter1.getTransactionDate();
        transactionAmount=geter1.getTransactionAmount();
        transactionMode=geter1.getTransactionMode();

        TransactionId.setText(transactionId);
        TransactionDate.setText(transactionDate);
        TransactionAmount.setText(transactionAmount);
        TransactionMode.setText(transactionMode);

    }

    @Override
    public int getItemCount() {
       return data.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        public ViewHolder(View itemView) {
            super(itemView);

            TransactionId=(TextView)itemView.findViewById(R.id.TransactionId);
            TransactionDate=(TextView)itemView.findViewById(R.id.TransactionDate);
            TransactionAmount=(TextView)itemView.findViewById(R.id.TransactionAmount);
            TransactionMode=(TextView)itemView.findViewById(R.id.TransactionMode);

        }
    }
}
