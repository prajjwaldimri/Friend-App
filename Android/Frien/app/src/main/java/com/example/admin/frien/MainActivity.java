package com.example.admin.frien;

import android.app.Activity;
import android.support.v7.app.ActionBarActivity;
import android.support.v7.app.ActionBar;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.content.Context;
import android.os.Build;
import android.os.Bundle;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.support.v4.widget.DrawerLayout;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Button;
import android.widget.RadioButton;
import android.content.Intent;
import android.net.Uri;
import android.widget.Toast;
import android.content.Intent;
import android.telephony.SmsManager;
import java.io.IOException;
import android.media.MediaPlayer;
import android.media.MediaRecorder;
import android.os.Environment;
import android.location.Criteria;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;

import android.content.Context;

public class MainActivity extends ActionBarActivity implements NavigationDrawerFragment.NavigationDrawerCallbacks , LocationListener {
    LocationManager locationmanager;
Button b,play,stop,record;
    private MediaRecorder myaudiorecorder;
    private String OutputFile=null;


    /**
     * Fragment managing the behaviors, interactions and presentation of the navigation drawer.
     */
    private NavigationDrawerFragment mNavigationDrawerFragment;

    /**
     * Used to store the last screen title. For use in {@link #restoreActionBar()}.
     */
    private CharSequence mTitle;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        b=(Button)findViewById(R.id.button5);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String phoneNumber = "tel:7830207022";
                String message = "SMS from Friend";
                try {
                    SmsManager smsManager = SmsManager.getDefault();
                    smsManager.sendTextMessage(phoneNumber, null,message, null, null);
                    Toast.makeText(getApplicationContext(), "SMS Sent!", Toast.LENGTH_SHORT).show();
                } catch (Exception e) {
                    Toast.makeText(getApplicationContext(),
                            "SMS failed, please try again later!",
                            Toast.LENGTH_SHORT).show();
                    e.printStackTrace();
                }
                Intent in = new Intent(Intent.ACTION_CALL, Uri.parse("tel:" + "7830207022"));
                startActivity(in);


            }
        });
        locationmanager=(LocationManager)getSystemService(Context.LOCATION_SERVICE);
        Criteria cri=new Criteria();
        String provider=locationmanager.getBestProvider(cri,false);

        if(provider!=null & !provider.equals(""))
        {
            Location location=locationmanager.getLastKnownLocation(provider);
            locationmanager.requestLocationUpdates(provider,2000,1, (LocationListener) MainActivity.this);
            if(location!=null)
            {
                onLocationChanged(location);
            }
            else{
                Toast.makeText(getApplicationContext(),"location not found",Toast.LENGTH_LONG ).show();
            }
        }
        else
        {
            Toast.makeText(getApplicationContext(),"Provider is null",Toast.LENGTH_LONG).show();
        }
        play=(Button)findViewById(R.id.button3);
        stop=(Button)findViewById(R.id.button2);
        record=(Button)findViewById(R.id.button4);

        stop.setEnabled(false);
        play.setEnabled(false);
        OutputFile = Environment.getExternalStorageDirectory().getAbsolutePath() + "/recording.3gp";;
        myaudiorecorder=new MediaRecorder();
        myaudiorecorder.setAudioSource(MediaRecorder.AudioSource.MIC);
        myaudiorecorder.setOutputFormat(MediaRecorder.OutputFormat.THREE_GPP);
        myaudiorecorder.setAudioEncoder(MediaRecorder.OutputFormat.AMR_NB);
        myaudiorecorder.setOutputFile(OutputFile);
record.setOnClickListener(new View.OnClickListener() {
    @Override
    public void onClick(View v) {
        try {
            myaudiorecorder.prepare();
            myaudiorecorder.start();
        } catch (IllegalStateException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        record.setEnabled(false);
        stop.setEnabled(true);

        Toast.makeText(getApplicationContext(), "Recording started", Toast.LENGTH_LONG).show();
    }
});
    stop.setOnClickListener(new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            myaudiorecorder.stop();
            myaudiorecorder.release();
            myaudiorecorder = null;

            stop.setEnabled(false);
            play.setEnabled(true);

            Toast.makeText(getApplicationContext(), "Audio recorded successfully", Toast.LENGTH_LONG).show();
        }
    });
play.setOnClickListener(new View.OnClickListener() {
    @Override
    public void onClick(View v) throws IllegalArgumentException,SecurityException,IllegalStateException  {
        MediaPlayer m = new MediaPlayer();

        try {
            m.setDataSource(OutputFile);
        }

        catch (IOException e) {
            e.printStackTrace();
        }

        try {
            m.prepare();
        }

        catch (IOException e) {
            e.printStackTrace();
        }

        m.start();
        Toast.makeText(getApplicationContext(), "Playing audio", Toast.LENGTH_LONG).show();

    }
});

        mNavigationDrawerFragment = (NavigationDrawerFragment)
                getSupportFragmentManager().findFragmentById(R.id.navigation_drawer);
        mTitle = getTitle();

        // Set up the drawer.
        mNavigationDrawerFragment.setUp(
                R.id.navigation_drawer,
                (DrawerLayout) findViewById(R.id.drawer_layout));
    }
@Override
    public void onLocationChanged(Location location) {
        TextView textView2=(TextView)findViewById(R.id.textview2);

        TextView textView3=(TextView)findViewById(R.id.textview3);

        textView2.setText("Latitude"+location.getLatitude());
        textView3.setText("Longitude"+ location.getLongitude());
    }
@Override
    public void onStatusChanged(String s, int i, Bundle bundle) {
    }
    @Override
    public void onProviderEnabled(String s) {
    }
    @Override

    public void onProviderDisabled(String s) {
    }

    @Override
    public void onNavigationDrawerItemSelected(int position) {
        // update the main content by replacing fragments
        FragmentManager fragmentManager = getSupportFragmentManager();
        fragmentManager.beginTransaction()
                .replace(R.id.container, PlaceholderFragment.newInstance(position + 1))
                .commit();
    }

    public void onSectionAttached(int number) {
        switch (number) {
            case 1:
                mTitle = "Message";
                break;
            case 2:
                mTitle = "Call";
                break;
            case 3:
                mTitle = "Setting";
                break;
            case 4:
                mTitle = "SOS";
                Intent i1 = new Intent(this,SosActivity.class);
                startActivity(i1);
                break;
        }
    }

    public void restoreActionBar() {
        ActionBar actionBar = getSupportActionBar();
        actionBar.setNavigationMode(ActionBar.NAVIGATION_MODE_STANDARD);
        actionBar.setDisplayShowTitleEnabled(true);
        actionBar.setTitle(mTitle);
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu
    ) {
        if (!mNavigationDrawerFragment.isDrawerOpen()) {
            // Only show items in the action bar relevant to this screen
            // if the drawer is not showing. Otherwise, let the drawer
            // decide what to show in the action bar.
            getMenuInflater().inflate(R.menu.main, menu);
            restoreActionBar();
            return true;
        }
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    public void callbutton(View view) {
        Intent in = new Intent(Intent.ACTION_CALL, Uri.parse("tel:" + "7830207022"));
        startActivity(in);

    }

    public void callers(View view) {
        /*Intent sendIntent = new Intent(Intent.ACTION_VIEW);
        sendIntent.putExtra("sms_body", "Content of the SMS goes here...");

        sendIntent.setType("vnd.android-dir/mms-sms");
        startActivity(sendIntent);*/
        String phoneNumber = "tel:7830207022";
        String message = "SMS from Friend";
        try {
            SmsManager smsManager = SmsManager.getDefault();
            smsManager.sendTextMessage(phoneNumber, null,message, null, null);
            Toast.makeText(getApplicationContext(), "SMS Sent!", Toast.LENGTH_SHORT).show();
        } catch (Exception e) {
            Toast.makeText(getApplicationContext(),
                    "SMS faild, please try again later!",
                    Toast.LENGTH_SHORT).show();
            e.printStackTrace();

        }
    }






    /**
     * A placeholder fragment containing a simple view.
     */
    public static class PlaceholderFragment extends Fragment {
        /**
         * The fragment argument representing the section number for this
         * fragment.
         */
        private static final String ARG_SECTION_NUMBER = "section_number";

        /**
         * Returns a new instance of this fragment for the given section
         * number.
         */
        public static PlaceholderFragment newInstance(int sectionNumber) {
            PlaceholderFragment fragment = new PlaceholderFragment();
            Bundle args = new Bundle();
            args.putInt(ARG_SECTION_NUMBER, sectionNumber);
            fragment.setArguments(args);
            return fragment;
        }

        public PlaceholderFragment() {
        }

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            View rootView = inflater.inflate(R.layout.fragment_main, container, false);
            return rootView;
        }

        @Override
        public void onAttach(Activity activity) {
            super.onAttach(activity);
            ((MainActivity) activity).onSectionAttached(
                    getArguments().getInt(ARG_SECTION_NUMBER));
        }
    }

}
