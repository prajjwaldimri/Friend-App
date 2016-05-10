package com.example.admin.friend;

import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.preference.PreferenceManager;
import android.telephony.SmsManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import java.util.concurrent.TimeUnit;


public class TimerPageFragment extends android.support.v4.app.Fragment {
Button holdButton;
    TextView _timer;
    View view;


    public TimerPageFragment(){

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view= inflater.inflate(R.layout.fragment_timerpage, container, false);
        holdButton=(Button)view.findViewById(R.id.button7);
        _timer=(TextView)view.findViewById(R.id.textView9);
        final CountDownTimer timer=new CountDownTimer(180000,100) {
            @Override
            public void onTick(long millisUntilFinished) {
                String _msm=String.format("%d:%03d:%03d", TimeUnit.MILLISECONDS.toMinutes(millisUntilFinished), TimeUnit.MILLISECONDS.toSeconds(millisUntilFinished)-TimeUnit.MINUTES.toSeconds(TimeUnit.MILLISECONDS.toMinutes(millisUntilFinished)), TimeUnit.MILLISECONDS.toMillis(millisUntilFinished)-TimeUnit.SECONDS.toMillis(TimeUnit.MILLISECONDS.toSeconds(millisUntilFinished)));
                _timer.setText(_msm);
            }

            @Override
            public void onFinish() {
            spine();

            }
        };
       holdButton.setOnLongClickListener(new View.OnLongClickListener() {
           @Override
           public boolean onLongClick(View v) {
               timer.start();
               return true;
           }
       });
        SettingPageFragment settingPageFragment=new SettingPageFragment();
       settingPageFragment._themeShared= PreferenceManager.getDefaultSharedPreferences(getActivity().getApplicationContext());
        settingPageFragment._themecolor=settingPageFragment._themeShared.toString();
        if (settingPageFragment._themecolor.equals(settingPageFragment._themeShared)) {
            getActivity().setTheme(Integer.parseInt(settingPageFragment._themecolor));
        }
        return  view;
    }
    public void spine() {
        String phoneNumber = "tel:7830207022";
        String message = "SMS from Friend";
        try {
            SmsManager smsManager = SmsManager.getDefault();
            smsManager.sendTextMessage(phoneNumber, null, message, null, null);
            Toast.makeText(getContext().getApplicationContext(), "SMS Sent!", Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Toast.makeText(getContext().getApplicationContext(), "SMS failed, please try again later!", Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
        Intent in = new Intent(Intent.ACTION_CALL, Uri.parse("tel:" + "7830207022"));
        startActivity(in);



    }

}
