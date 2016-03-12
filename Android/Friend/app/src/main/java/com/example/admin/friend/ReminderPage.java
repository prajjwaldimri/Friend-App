package com.example.admin.friend;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TimePicker;

<<<<<<< HEAD:Android/Friend/app/src/main/java/com/example/admin/friend/ReminderPAge.java
public class ReminderPAge extends android.support.v4.app.Fragment {
View view;
=======
public class ReminderPage extends android.support.v4.app.Fragment {
    View view;
>>>>>>> 8210d47990cac17563db65e6282c9c312f314f60:Android/Friend/app/src/main/java/com/example/admin/friend/ReminderPage.java
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
