package com.example.admin.friend;

import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class OAuth_Activity extends FragmentActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_oauth_);
        String authenticationUrl = getIntent().getStringExtra(ConstantValues.STRING_EXTRA_AUTHENCATION_URL);
        FragmentTransaction fragmentTransaction = getSupportFragmentManager().beginTransaction();
        Auth_Activity auth_activity = new Auth_Activity(authenticationUrl);
        fragmentTransaction.add(android.R.id.content,auth_activity);
        fragmentTransaction.commit();
    }
}
