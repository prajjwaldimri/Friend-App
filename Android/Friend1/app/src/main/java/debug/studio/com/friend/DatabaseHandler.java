package debug.studio.com.friend;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.provider.Contacts;
import android.provider.ContactsContract;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by DELL on 01-Nov-15.
 */
public class DatabaseHandler extends SQLiteOpenHelper {
    private static final int DATABASE_VERSION=1;
    private  static final String DATABASE_NAME="contactsManager";
    private static final String TABLE_CONTACTS="contacts";
    private static final String KEY_ID="id";
    private static final String KEY_NAME="name";
    private static final String KEY_PH_NO="phone number";

    public DatabaseHandler(Main22Activity context) {
        super(context,DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        String CREATE_CONTSCTS_TABLE="CREATE TABLE"+TABLE_CONTACTS+"("+KEY_ID+"INTEGER PRIMARY KEY,"+KEY_NAME+"TEXT,"+KEY_PH_NO+"TEXT"+")";
        db.execSQL(CREATE_CONTSCTS_TABLE);

    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS"+TABLE_CONTACTS);
onCreate(db);
    }
    public void addContact(Contacts contacts){
        SQLiteDatabase db=this.getWritableDatabase();
        ContentValues values=new ContentValues();
        values.put(KEY_NAME,contacts.getName());
        values.put(KEY_PH_NO,contacts.getPhoneNumber());
        db.insert(TABLE_CONTACTS, null, values);
        db.close();
    }
    public Contacts getContact(int id){
        SQLiteDatabase db=this.getReadableDatabase();
        Cursor cursor=db.query(TABLE_CONTACTS,new String[]{KEY_ID,KEY_NAME,KEY_PH_NO},KEY_ID+"=?",new String[]{String.valueOf(id)},null,null,null,null);
        if (cursor!=null)
            cursor.moveToFirst();;
        Contacts contacts=new Contacts(Integer.parseInt(cursor.getString(0)),cursor.getString(1),cursor.getString(2));
        return contacts;
    }
    public List<Contacts> getAllContacts(){
        List<Contacts > contactsList=new ArrayList<Contacts>();
        String selectQuery="SELECT *FROM"+TABLE_CONTACTS;
        SQLiteDatabase db=this.getWritableDatabase();
        Cursor cursor=db.rawQuery(selectQuery,null);
        if (cursor.moveToFirst()){
            do{
                Contact contacts=new Contact();
                contacts.setID(Integer.parseInt(cursor.getString(0)));
                contacts.setName(cursor.getString(1));
                contacts.setPhoneNumber(cursor.getString(2));
                contactsList.add(contacts);

            }while (cursor.moveToNext());

        }
        return contactsList;
    }
    public int updateContact(Contacts contacts){
        SQLiteDatabase db=this.getWritableDatabase();ContentValues values=new ContentValues();
        values.put(KEY_NAME,contacts.getName());
        values.put(KEY_PH_NO,contacts.getPhoneNumber());
        return  db.update(TABLE_CONTACTS,values,KEY_ID+"=?",new String[]{String.valueOf(contacts.getID())});

    }
    public  void deleteContact(Contacts contacts){
        SQLiteDatabase db=this.getWritableDatabase();
        db.delete(TABLE_CONTACTS, KEY_ID + "=?", new String[]{String.valueOf(contacts.grtID())});
        db.close();
    }
    public int getContactsCount(){
        String countQuery="SELECT * FROM"+TABLE_CONTACTS;
        SQLiteDatabase db=this.getReadableDatabase();Cursor cursor=db.rawQuery(countQuery,null);
        cursor.close();
        return  cursor.getCount();
    }


    private class Contact {
    }
}
