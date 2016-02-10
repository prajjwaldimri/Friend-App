package com.example.admin.friend;

import android.app.DownloadManager;
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.design.widget.NavigationView;
import android.support.v4.app.FragmentActivity;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.OutputStream;

public class OneFragment extends android.support.v4.app.Fragment {
    private static final int RESULT_OK = 1;
    View view;

    Bitmap bmp;
    ImageView iv;
    public OneFragment() {

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_one, container, false);
        iv = (ImageView) view.findViewById(R.id.imageView);
        iv.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openGallery();
            }
        });


        return view;
    }

    private void openGallery() {
        Intent photoPickerIntent = new Intent(Intent.ACTION_GET_CONTENT);
        photoPickerIntent.setType("image/*");
        startActivityForResult(photoPickerIntent, 1);

    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        try{
            if (requestCode==RESULT_OK){
                String path=getPathFromCameraData(data,this.getActivity());
                Bitmap bmp=BitmapFactory.decodeFile(path);
                iv.setImageBitmap(bmp);

            }else {
                Toast.makeText(getActivity(), "Pick your image first", Toast.LENGTH_LONG).show();
            }
        }catch (Exception e){
            e.printStackTrace();

        }
}

    private String getPathFromCameraData(Intent data, Context context) {
        Uri selectImage=data.getData();
        String[] filepathColumn={MediaStore.Images.Media.DATA};
        Cursor cursor=context.getContentResolver().query(selectImage, filepathColumn, null, null, null);
        assert cursor != null;
        cursor.moveToFirst();
        int columnIndx=cursor.getColumnIndex(filepathColumn[0]);
        String piturepath=cursor.getString(columnIndx);
        cursor.close();
        return piturepath;
    }
    }

















