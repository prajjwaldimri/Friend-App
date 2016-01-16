package com.example.admin.friend;

import android.app.DownloadManager;
import android.content.ContentResolver;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.design.widget.NavigationView;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;

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

    private ContentResolver contentResolver;

    public OneFragment() {

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        retrieve();

    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_one, container, false);
        iv=(ImageView)view.findViewById(R.id.imageView);
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
        if (requestCode == 1) {
            if (data != null && resultCode == RESULT_OK) {
                Uri selectedImage = data.getData();
                String[] filePathColumn = {MediaStore.Images.Media.DATA};
                Cursor cursor= getContentResolver().query(selectedImage,filePathColumn,null,null,null);
                cursor.moveToFirst();
                int columnIndex=cursor.getColumnIndex(filePathColumn[0]);
                String picturePath=cursor.getString(columnIndex);
                cursor.close();
                if (bmp!=null&&!bmp.isRecycled()){
                    bmp=null;
                }
                bmp=BitmapFactory.decodeFile(picturePath);
                iv.setImageBitmap(bmp);

            } else {
                Log.d("Status:", "Photopicker canceled");
            }

            storeimage();

        }

    }

    private boolean storeimage() {
        ByteArrayOutputStream bytes = new ByteArrayOutputStream();
        String filepaths = Environment.getExternalStorageDirectory() + "/friend2/myProfile/";
        File image = new File(filepaths);
        image.mkdirs();
        try {
            FileOutputStream fo = new FileOutputStream(filepaths);
            if (bmp != null) {

                bmp.compress(Bitmap.CompressFormat.JPEG, 100, bytes);
            } else {
                return false;
            }

        } catch (FileNotFoundException e1) {
            e1.printStackTrace();
        }
        return true;
    }
    public boolean retrieve(){
        File f = new File(android.os.Environment.getExternalStorageDirectory() + "/friend2/myProfile/");
        Bitmap bmp = BitmapFactory.decodeFile(f.getAbsolutePath());
        iv.setImageBitmap(bmp);
        return  true;

    }

    public ContentResolver getContentResolver() {
        return contentResolver;
    }
}















