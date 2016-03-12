package com.example.admin.friend;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TimePicker;

public class ReminderPage extends android.support.v4.app.Fragment {
    View view;
    TimePicker timePicker;
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view= inflater.inflate(R.layout.fragment_reminder, container, false);
        timePicker=(TimePicker)view.findViewById(R.id.timePicker);
        return view;
    }
}
