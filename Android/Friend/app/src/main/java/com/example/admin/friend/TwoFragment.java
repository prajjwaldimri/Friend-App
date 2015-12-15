package com.example.admin.friend;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.Switch;
import android.widget.Toast;

public class TwoFragment extends android.support.v4.app.Fragment{
    View view;
    Switch s1,s2,s3;
    EditText et,et1,et2,et3,et4;
    Button b;
    public TwoFragment(){

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view=inflater.inflate(R.layout.fragment_two,container,false);
        s1=(Switch)view.findViewById(R.id.switch1);
        s2=(Switch)view.findViewById(R.id.switch2);
        s3=(Switch)view.findViewById(R.id.switch3);


        s1.setChecked(true);

        s2.setChecked(true);
        s3.setChecked(true);
        s1.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (s1.isChecked() == false) {


                    Toast.makeText(getActivity(), "Message feature will not work properly", Toast.LENGTH_LONG).show();
                }
            }
        });
        s2.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (s2.isChecked() == false) {

                    Toast.makeText(getActivity(), "Calling feature will not work properly", Toast.LENGTH_LONG).show();
                }
            }
        });
        s3.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {

            @Override
            public void onCheckedChanged (CompoundButton buttonView,boolean isChecked){
                if (s3.isChecked() == false) {

                    Toast.makeText(getActivity(), "Location  feature will not work properly", Toast.LENGTH_LONG).show();
                }
            }
        });
        et=(EditText)view.findViewById(R.id.editText);
        et1=(EditText)view.findViewById(R.id.editText2);
        et2=(EditText)view.findViewById(R.id.editText3);
        et3=(EditText)view.findViewById(R.id.editText4);
        et4=(EditText)view.findViewById(R.id.editText5);
        b=(Button)view.findViewById(R.id.button);

        b.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                String num=et.getText().toString();
                String num1=et1.getText().toString();
                String num2=et2.getText().toString();
                String num3=et3.getText().toString();
                String num4 =et4.getText().toString();

            }
        });






        return view;
    }
}

