package com.example.admin.friend;

import android.content.Context;
import android.content.ContextWrapper;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.PorterDuff;
import android.graphics.PorterDuffXfermode;
import android.graphics.Rect;
import android.graphics.RectF;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Random;

public class HomePagefragment extends android.support.v4.app.Fragment {
    private static final int RESULT_OK = 1;
    View view;
TextView location;
    Bitmap bmp;
    ImageView iv;
    public HomePagefragment() {

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_homepage, container, false);
        iv = (ImageView) view.findViewById(R.id.profileImageView);
        location=(TextView)view.findViewById(R.id.textView9);
        iv.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openGallery();

            }
        });

  retrieveImage();



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
        try
        {
            if (requestCode==RESULT_OK){
              String path=getPathFromCameraData(data,getActivity());
                Bitmap bmp=BitmapFactory.decodeFile(path);
                iv.setImageBitmap(bmp);
                storeImage(bmp);

            }
    } catch (IOException e) {
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
    private boolean storeImage(Bitmap bitmap) throws IOException {
        OutputStream outputStream=null ;
        String directory=Environment.getExternalStorageDirectory().toString();



        File file=new File(directory,"/friend/profile.jpg");
        if (file.exists())file.delete();
        try{


            outputStream = new FileOutputStream(file);
            bitmap.compress(Bitmap.CompressFormat.JPEG, 100, outputStream);
            outputStream.close();
            MediaStore.Images.Media.insertImage(getActivity().getContentResolver(),file.getAbsolutePath(),file.getName(),file.getName());
        }
        catch (Exception e){
            e.printStackTrace();
        }



        return true;

        }
    public boolean retrieveImage(){
        File f = new File( "/sdcard/friend/profile.jpg");
        if (f.exists()) {
            Bitmap bmp = BitmapFactory.decodeFile(f.getAbsolutePath());
            iv.setImageBitmap(bmp);
        }
            return true;

    }

}