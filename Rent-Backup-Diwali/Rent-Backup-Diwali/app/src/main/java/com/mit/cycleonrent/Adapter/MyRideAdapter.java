package com.mit.cycleonrent.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;


import androidx.recyclerview.widget.RecyclerView;

import com.mit.cycleonrent.MyRide;
import com.mit.cycleonrent.R;

import java.util.List;

public class MyRideAdapter extends RecyclerView.Adapter<MyRideAdapter.ViewHolder> {
    Context context;
    List<MyRide> data;
    TextView RideDate;
    TextView DeviceId;
    TextView RideDuration;
    TextView RideFromTime;
    TextView RideToTime;
    TextView RideFromLocation;
    TextView RideToLocation;
    TextView RideDistance;
    TextView RideCost;
    TextView RideRate;

    public MyRideAdapter(List<MyRide> data, Context context)
    {
        this.context=context;
        this.data=data;
    }
    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
       View view=LayoutInflater.from(parent.getContext()).inflate(R.layout.content_myrides_card,parent,false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        final MyRide geter1 =  data.get(position);
        String deviceId,rideRate,rideDate, rideDuration,rideFromTime,rideToTime,rideFromLocation,rideToLocation,rideDistance,rideCost;

        deviceId=geter1.getDeviceId();
        rideDate=geter1.getRideDate();
        rideDuration=geter1.getRideDuration();
        rideFromTime=geter1.getRideFromTime();
        rideToTime=geter1.getRideToTime();
        rideFromLocation=geter1.getRideFromLocation();
        rideToLocation=geter1.getRideToLocation();
        rideDistance=geter1.getRideDistance();
        rideCost=geter1.getRideCost();
        rideRate=geter1.getRideRate();

        DeviceId.setText(deviceId);
        RideDate.setText(rideDate);
        RideDuration.setText(rideDuration);
        RideFromTime.setText(rideFromTime);
        RideFromLocation.setText(rideFromLocation);
        RideToLocation.setText(rideToLocation);
        RideDistance.setText(rideDistance);
        RideCost.setText(rideCost);
        RideToTime.setText(rideToTime);
        RideRate.setText(rideRate);



    }

    @Override
    public int getItemCount() {
       return data.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        public ViewHolder(View itemView) {
            super(itemView);

            DeviceId=(TextView)itemView.findViewById(R.id.DeviceId);
            RideDate=(TextView)itemView.findViewById(R.id.RideDate);
            RideDuration=(TextView)itemView.findViewById(R.id.RideDuration);
            RideFromTime=(TextView)itemView.findViewById(R.id.RideFromTime);
            RideFromLocation=(TextView)itemView.findViewById(R.id.RideFromLocation);
            RideToLocation=(TextView)itemView.findViewById(R.id.RideToLocation);
            RideDistance=(TextView)itemView.findViewById(R.id.RideDistance);
            RideCost=(TextView)itemView.findViewById(R.id.RideCost);
            RideToTime=(TextView)itemView.findViewById(R.id.RideToTime);
            RideRate=(TextView)itemView.findViewById(R.id.RideRate);


        }
    }
}
