package com.example.admin.friend;

import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.telephony.SmsManager;
import android.widget.Toast;

/**
 * Created by ayush on 13/3/16.
 */
public class BackgroundTask extends AsyncTask{
    private long time;
    @Override
    protected Object doInBackground(Object[] params) {
        try {
            Thread.sleep(200000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return null;
    }

    @Override
    protected void onPostExecute(Object o) {
        super.onPostExecute(o);

Intent intent=new Intent();
    }


    @Override
    protected void onProgressUpdate(Object[] values) {
        super.onProgressUpdate(values);
    }

    @Override
    protected void onPreExecute() {

        super.onPreExecute();
        time=System.currentTimeMillis();
    }
}
