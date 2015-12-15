package com.example.admin.friend;

import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
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

public class OneFragment extends android.support.v4.app.Fragment
{
    View view;
    Button b;
    Bitmap bmp;
    ImageView iv;

    public OneFragment(){

    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }


    @Override
    public  View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_one, container, false);
        iv = (ImageView) view.findViewById(R.id.imageView2);
        b = (Button) view.findViewById(R.id.button2);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openGallery();
            }
        });
    return view;}




    private void openGallery() {
        Intent photoPickerIntent=new Intent(Intent.ACTION_GET_CONTENT);
        photoPickerIntent.setType("image/*");
        startActivityForResult(photoPickerIntent, 1);

    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode==1){
            if (data!=null&&resultCode==RESULT_OK){
                Uri selectedImage=data.getData();
                String[] filePathColumn={MediaStore.Images.Media.DATA};
                Cursor cursor=getContentResolver().query(selectedImage, filePathColumn, null, null, null);
                cursor.moveToFirst();
                int columnIndex=cursor.getColumnIndex(filePathColumn[0]);
                String filePath=cursor.getString(columnIndex);
                cursor.close();
                if ((bmp != null) && !bmp.isRecycled()){
                    bmp=null;
                }
                bmp= BitmapFactory.decodeFile(filePath);
                iv.setBackgroundResource(0);
                iv.setImageBitmap(bmp);

            }
            else {
                Log.d("Status:", "Photopicker canceled");
            }
        }

    }

}