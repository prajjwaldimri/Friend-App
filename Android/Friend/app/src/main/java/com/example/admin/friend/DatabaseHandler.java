package com.example.admin.friend;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.widget.EditText;

/**
 * Created by DELL on 06-Jan-16.
 */
class Contact{
    int _id;
    String _name;
    String _phone_number;
    public Contact(String et){}
    public Contact(int id, String name, String _phone_number){
        this._id=id;
        this._name=name;
        this._phone_number=_phone_number;
    }
    public Contact(String name, String _phone_number){
        this._name=name;
        this._phone_number=_phone_number;
    }
    public int getID(){
        return this._id;
    }
    public void set_id(int id){
        this._id=id;
    }
    public String get_name(){
        return this._name;
    }
    public void set_name(String name){
        this._name=name;
    }
    public String get_phone_number(){
        return this._phone_number;
    }
    public void set_phone_number(String phone_number){
        this._phone_number=phone_number;
    }
}
public class DatabaseHandler extends SQLiteOpenHelper {
    private static final int DATABASE_VERSION=1;
    private static final String DATABASE_NAME="ContactManger";
    private static final String TABLE_NAME="contacts";
    private static final String KEY_ID="id";
    private static final String KEY_NAME="name";
    private static final String KEY_PHONE="phone_number";

    public DatabaseHandler(Context context) {

        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }




    @Override
    public void onCreate(SQLiteDatabase db) {
        String CREATE_TABLE="CREATE TABLE"+TABLE_NAME+"("+KEY_ID+"INTEGER PRIMARY KEY,"+KEY_NAME+"TEXT"+KEY_PHONE+"TEXT"+")";
        db.execSQL(CREATE_TABLE);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS"+TABLE_NAME);
        onCreate(db);

    }
    //adding com.example.admin.friend.Contact
    public void addContact(Contact contact){
        SQLiteDatabase db=this.getWritableDatabase();
        ContentValues values=new ContentValues();
        values.put(KEY_NAME,contact.get_name());
        values.put(KEY_PHONE,contact.get_phone_number());
        db.insert(TABLE_NAME, null, values);
        db.close();
    }
    //Read contact
    public Contact getContact(int id){
        SQLiteDatabase db=this.getReadableDatabase();
        Cursor cursor=db.query(TABLE_NAME,new String[]{KEY_ID,KEY_NAME,KEY_PHONE},KEY_ID+"=?",new String[]{String.valueOf(id)},null,null,null,null);
        if (cursor!=null){
            cursor.moveToFirst();

        }
        assert cursor != null;
        return new Contact(Integer.parseInt(cursor.getString(0)),cursor.getString(1),cursor.getString(2));
    }
    //update single contact
    public int updateContact(Contact contact){
        SQLiteDatabase db=this.getWritableDatabase();
        ContentValues values=new ContentValues();
        values.put(KEY_NAME,contact.get_name());
        values.put(KEY_PHONE,contact.get_phone_number());
        return db.update(TABLE_NAME,values,KEY_ID+"=?",new String[]{String.valueOf(contact.getID())});
    }
    //delete single contact
    public void deleteContact(Contact contact){
        SQLiteDatabase db=this.getWritableDatabase();
        db.delete(TABLE_NAME,KEY_ID+"=?",new String[]{String.valueOf(contact.getID())});
        db.close();
    }
}
