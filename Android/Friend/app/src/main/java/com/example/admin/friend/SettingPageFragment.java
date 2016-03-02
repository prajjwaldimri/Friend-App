package com.example.admin.friend;


import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.Switch;
import android.widget.TextView;

import static android.provider.ContactsContract.CommonDataKinds.Phone;

public class SettingPageFragment extends android.support.v4.app.Fragment implements View.OnClickListener {
    private static final int CONTACT_PICKER = 1001;
    private static final int RESULT_OK = 1;
    Notification mynotification;
    View view;
    Switch theme, toast;
    TextView pname,phoneno,tv3,tv4;
    Button contact1,contact2, contact3,contact4,Register;
    private Context context;


    public SettingPageFragment() {
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_settingpage, container, false);

        toast = (Switch) view.findViewById(R.id.switch1);
        theme = (Switch) view.findViewById(R.id.switch2);
        //s3 = (Switch) view.findViewById(R.id.switch3);


/*
        contact1 = (Button) view.findViewById(R.id.button);
        contact2 = (Button) view.findViewById(R.id.button3);
        contact3= (Button) view.findViewById(R.id.button5);
        contact4=(Button)view.findViewById(R.id.button4);
        Register=(Button)view.findViewById(R.id.button2);
        pname = (TextView) view.findViewById(R.id.textView);
        phoneno = (TextView) view.findViewById(R.id.textView2);
        tv3=(TextView)view.findViewById(R.id.textView3);
        tv4=(TextView)view.findViewById(R.id.textView5);


        contact1.setOnClickListener(this);
        contact2.setOnClickListener(this);
        contact3.setOnClickListener(this);
        contact4.setOnClickListener(this);
        Register.setOnClickListener(this);
*/
        toast.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (toast.isChecked()) {
                    String title = "Friend";
                    String subject = "Notification by friend app";
                    String body = "notification";
                    NotificationManager manager = (NotificationManager) getActivity().getSystemService(Context.NOTIFICATION_SERVICE);
                    Notification.Builder builder = new Notification.Builder(getActivity());
                    builder.setContentTitle(title);
                    builder.setContentText(subject);
                    builder.setSmallIcon(R.mipmap.ic_launcher);
                    builder.setOngoing(true);
                    builder.build();
                    PendingIntent pendingIntent = PendingIntent.getActivity(getContext(), 0, new Intent(), 0);
                    mynotification = builder.getNotification();
                    manager.notify(11, mynotification);

                } else {

                }
            }
        });


        return view;


    }


  @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.button2:
                Intent contactpicker = new Intent(Intent.ACTION_PICK, Phone.CONTENT_URI);
                startActivityForResult(contactpicker, CONTACT_PICKER);
                break;
            case R.id.button3:
                contactpicker = new Intent(Intent.ACTION_PICK, Phone.CONTENT_URI);
                startActivityForResult(contactpicker, CONTACT_PICKER);
                break;
            case R.id.button4:
                contactpicker = new Intent(Intent.ACTION_PICK, Phone.CONTENT_URI);
                startActivityForResult(contactpicker, CONTACT_PICKER);
                break;
            case R.id.button5:
                contactpicker = new Intent(Intent.ACTION_PICK, Phone.CONTENT_URI);
                startActivityForResult(contactpicker, CONTACT_PICKER);
                break;


        }


    }
/*
    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode == RESULT_OK) {
            switch (requestCode) {
                case CONTACT_PICKER:
                    contactPicked(data);
                    break;
            }
        }
    }   private void contactPicked(Intent data) {
        Cursor c;
        try{
            String phoneNo;
            String name;
            Uri uri=data.getData();
           c= context.getContentResolver().query(uri,null,null,null,null);
            assert c != null;
            c.moveToFirst();
            int phoneIndx=c.getColumnIndex(Phone.NUMBER);
            int nameIndx=c.getColumnIndex(Phone.DISPLAY_NAME);
            phoneNo=c.getString(phoneIndx);
            name=c.getString(nameIndx);
            pname.setText(name);
            phoneno.setText(phoneNo);
        }catch (Exception e){
            e.printStackTrace();
        }
    }
*/



}






