package com.example.admin.friend;


import android.content.ContentResolver;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;


import java.sql.ResultSet;

import static android.provider.ContactsContract.CommonDataKinds.*;

public class SettingPageFragment extends android.support.v4.app.Fragment implements View.OnClickListener {
    private static final int CONTACT_PICKER = 1001;
    private static final int RESULT_OK = 1;

    View view;
    Switch s1, s2, s3;
    TextView tv1,tv2,tv3,tv4;
    Button b, b1, b2,b3,b4;
    private  ContentResolver contentResolver;


    public SettingPageFragment() {
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_settingpage, container, false);

        s1 = (Switch) view.findViewById(R.id.switch1);
        //s2 = (Switch) view.findViewById(R.id.switch2);
        //s3 = (Switch) view.findViewById(R.id.switch3);


        s1.setChecked(true);

        s2.setChecked(true);
        s3.setChecked(true);
        s1.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (!s1.isChecked()) {


                    Toast.makeText(getActivity(), "Message feature will not work properly", Toast.LENGTH_LONG).show();
                }
            }
        });
        s2.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (!s2.isChecked()) {

                    Toast.makeText(getActivity(), "Calling feature will not work properly", Toast.LENGTH_LONG).show();
                }
            }
        });
        s3.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {

            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (!s3.isChecked()) {

                    Toast.makeText(getActivity(), "Location  feature will not work properly", Toast.LENGTH_LONG).show();
                }
            }
        });

        b = (Button) view.findViewById(R.id.button);
        b1 = (Button) view.findViewById(R.id.button3);
        b2 = (Button) view.findViewById(R.id.button5);
        b3=(Button)view.findViewById(R.id.button4);
        b4=(Button)view.findViewById(R.id.button2);
        tv1 = (TextView) view.findViewById(R.id.textView);
        tv2 = (TextView) view.findViewById(R.id.textView2);
        tv3=(TextView)view.findViewById(R.id.textView3);
        tv4=(TextView)view.findViewById(R.id.textView5);


        b.setOnClickListener(this);
        b1.setOnClickListener(this);
        b2.setOnClickListener(this);
        b3.setOnClickListener(this);
        b4.setOnClickListener(this);


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
    }

    private void contactPicked(Intent data) {
        Cursor c;
        try{
            String phoneNo;
            String name;
            Uri uri=data.getData();
           c= getActivity().getContentResolver().query(uri,null,null,null,null);
            assert c != null;
            c.moveToFirst();
            int phoneIndx=c.getColumnIndex(Phone.NUMBER);
            int nameIndx=c.getColumnIndex(Phone.DISPLAY_NAME);
            phoneNo=c.getString(phoneIndx);
            name=c.getString(nameIndx);
            tv1.setText(name);
            tv2.setText(phoneNo);
        }catch (Exception e){
            e.printStackTrace();
        }
    }




}






