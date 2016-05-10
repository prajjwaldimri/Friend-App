package com.example.admin.friend;


import android.annotation.TargetApi;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import com.twitter.sdk.android.core.identity.OAuthActivity;
import com.twitter.sdk.android.core.identity.TwitterLoginButton;

import twitter4j.Twitter;
import twitter4j.auth.AccessToken;
import twitter4j.auth.RequestToken;

public class SettingPageFragment extends android.support.v4.app.Fragment implements View.OnClickListener {
    private TwitterLoginButton twitterLoginButton;
    private static final int CONTACT_PICKER = 1001;
    private static final int RESULT_OK = 1;
    Notification mynotification;
    View view;
    Switch theme, toast;
    TextView pname, phoneno, tv3, tv4;
    Button contact1, contact2, contact3, contact4, Register, _loginButton;
    private Context context;
    EditText _edittext;
    private boolean isUseStoredTokenKey = false;
    private boolean isUseWebViewForAuthentication = false;


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
        _edittext = (EditText) view.findViewById(R.id.edittext);
        _loginButton = (Button) view.findViewById(R.id.loginbutton);
        _loginButton.setOnClickListener(this);
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
            case R.id.loginbutton:
                if (ConstantValues.TWITTER_CONSUMER_KEY == null || ConstantValues.TWITTER_CONSUMER_SECRET == null) {
                    Toast.makeText(getActivity().getApplicationContext(), "Twitter oAuth infos:Please set your twitter consumer key and consumer secret", Toast.LENGTH_SHORT).show();
                    return;
                }
                else {
                    logIn();
                }
                break;
           /* case R.id.button2:
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
*/

        }


    }

    private void logIn() {

        SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(getActivity().getApplicationContext());
        if (!sharedPreferences.getBoolean(ConstantValues.PREFERENCE_TWITTER_IS_LOGGED_IN, false)) {
            new TwitterAuthenticateTask().execute();
        } else {
            initControl();
        }
    }

    class TwitterAuthenticateTask extends AsyncTask<String, String, RequestToken> {

        @Override
        protected void onPostExecute(RequestToken requestToken) {
            Intent intent = new Intent(getActivity().getApplicationContext(), OAuth_Activity.class);
            intent.putExtra(ConstantValues.STRING_EXTRA_AUTHENCATION_URL, requestToken.getAuthenticationURL());
            startActivity(intent);
        }
        @Override
        protected RequestToken doInBackground(String... params) {
            return TwitterUtil.getInstance().getRequestToken();
        }
    }

    private void initControl() {
        Uri uri = getActivity().getIntent().getData();
        if (uri != null && uri.toString().startsWith(ConstantValues.TWITTER_CALLBACK_URL)) {
            String verifier = uri.getQueryParameter(ConstantValues.URL_PARAMETER_TWITTER_OAUTH_VERIFIER);
            new TwitterGetAccessTokenTask().execute(verifier);
        } else {
            new TwitterGetAccessTokenTask().execute("");
        }
    }
    /*private void updateStatus(){
        String status = _edittext.getText().toString();
        new TwitterUpdateStatusTask().execute(status);
    }*/
    class TwitterGetAccessTokenTask extends AsyncTask<String, String, String> {

        @Override
        protected void onPostExecute(String userName) {
        }

        @Override
        protected String doInBackground(String... params) {

            Twitter twitter = TwitterUtil.getInstance().getTwitter();
            RequestToken requestToken = TwitterUtil.getInstance().getRequestToken();
            if (params[0]!=null) {
                try {

                    AccessToken accessToken = twitter.getOAuthAccessToken(requestToken, params[0]);
                    SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(getActivity().getApplicationContext());
                    SharedPreferences.Editor editor = sharedPreferences.edit();
                    editor.putString(ConstantValues.PREFERENCE_TWITTER_OAUTH_TOKEN, accessToken.getToken());
                    editor.putString(ConstantValues.PREFERENCE_TWITTER_OAUTH_TOKEN_SECRET, accessToken.getTokenSecret());
                    editor.putBoolean(ConstantValues.PREFERENCE_TWITTER_IS_LOGGED_IN, true);
                    editor.commit();
                    return twitter.showUser(accessToken.getUserId()).getName();
                } catch (twitter4j.TwitterException e) {
                    e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
                }
            } else {
                SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(getActivity().getApplicationContext());
                String accessTokenString = sharedPreferences.getString(ConstantValues.PREFERENCE_TWITTER_OAUTH_TOKEN, "");
                String accessTokenSecret = sharedPreferences.getString(ConstantValues.PREFERENCE_TWITTER_OAUTH_TOKEN_SECRET, "");
                AccessToken accessToken = new AccessToken(accessTokenString, accessTokenSecret);
                try {
                    TwitterUtil.getInstance().setTwitterFactory(accessToken);
                    return TwitterUtil.getInstance().getTwitter().showUser(accessToken.getUserId()).getName();
                } catch (twitter4j.TwitterException e) {
                    e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
                }
            }

            return null;  //To change body of implemented methods use File | Settings | File Templates.
        }
    }

    /*class TwitterUpdateStatusTask extends AsyncTask<String, String, Boolean> {

        @Override
        protected void onPostExecute(Boolean result) {
            if (result)
                Toast.makeText(getActivity().getApplicationContext(), "Tweet successfully", Toast.LENGTH_SHORT).show();
            else
                Toast.makeText(getActivity().getApplicationContext(), "Tweet failed", Toast.LENGTH_SHORT).show();
        }

        @Override
        protected Boolean doInBackground(String... params) {
            try {
                SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(getActivity().getApplicationContext());
                String accessTokenString = sharedPreferences.getString(ConstantValues.PREFERENCE_TWITTER_OAUTH_TOKEN, "");
                String accessTokenSecret = sharedPreferences.getString(ConstantValues.PREFERENCE_TWITTER_OAUTH_TOKEN_SECRET, "");

                if (accessTokenString!=null && accessTokenSecret!=null) {
                    AccessToken accessToken = new AccessToken(accessTokenString, accessTokenSecret);
                    twitter4j.Status status = TwitterUtil.getInstance().getTwitterFactory().getInstance(accessToken).updateStatus(params[0]);
                    return true;
                }

            } catch (twitter4j.TwitterException e) {
                e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
            }
            return false;  //To change body of implemented methods use File | Settings | File Templates.

        }
    }*/

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






