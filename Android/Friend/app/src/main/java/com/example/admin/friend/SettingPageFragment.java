package com.example.admin.friend;


import android.annotation.TargetApi;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.graphics.Typeface;
import android.os.Build;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.Switch;
import android.widget.TextView;

import static android.provider.ContactsContract.CommonDataKinds.Phone;
import static com.example.admin.friend.R.layout.fragment_settingpage;

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
        view = inflater.inflate(fragment_settingpage, container, false);

        toast = (Switch) view.findViewById(R.id.switch1);
        theme = (Switch) view.findViewById(R.id.switch2);
        //s3 = (Switch) view.findViewById(R.id.switch3);
        pname = (TextView) view.findViewById(R.id.textView);
        Typeface font=Typeface.createFromAsset(getActivity().getAssets(),"HirukoStencil.otf");
        Typeface font1=Typeface.createFromAsset(getActivity().getAssets(),"UBUNTU-R.TTF");
         pname.setTypeface(font);

        TextView textView=(TextView)view.findViewById(R.id.textView6);
        TextView textView2=(TextView)view.findViewById(R.id.textView8);
        TextView textView3=(TextView)view.findViewById(R.id.textView2);
        TextView textView4=(TextView)view.findViewById(R.id.textView4);
        TextView textView5=(TextView)view.findViewById(R.id.textView5);
        TextView textView6=(TextView)view.findViewById(R.id.textView7);
        Switch s1= (Switch) view.findViewById(R.id.switch1);
        TextView textView1=(TextView)view.findViewById(R.id.textView3);
        textView.setTypeface(font);
        textView1.setTypeface(font);
        textView2.setTypeface(font);
        textView3.setTypeface(font1);
        textView4.setTypeface(font1);
        textView5.setTypeface(font1);
        textView6.setTypeface(font1);
        s1.setTypeface(font1);

/*
        contact1 = (Button) view.findViewById(R.id.button);
        contact2 = (Button) view.findViewById(R.id.button3);
        contact3= (Button) view.findViewById(R.id.button5);
        contact4=(Button)view.findViewById(R.id.button4);
        Register=(Button)view.findViewById(R.id.sosNavigatorButton);
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
            @TargetApi(Build.VERSION_CODES.JELLY_BEAN)
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
            case R.id.sosNavigatorButton:
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






