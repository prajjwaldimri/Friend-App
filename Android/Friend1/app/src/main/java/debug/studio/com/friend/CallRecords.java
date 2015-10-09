package debug.studio.com.friend;
import android .content.Context;
import android.database.Cursor;
import android.content.ContentValues;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;



public class CallRecords {

    private static final int DATABASE_VERSION=1;
    private DatabaseHelper DBhelper;
    public SQLiteDatabase callDB;
    static final String DATABASE_NAME="DBLogs";
    static final String TABLE_NAME="Records";
    public static final String CALL_ROWID="_id";
    public static final String CALL_NUMBER="cnumber";
    public static final String CALL_DURATION="cduration";
    public static final String CALL_TYPE="ctype";
    Context context;
    private static final String Records="CREATE TABLE"+TABLE_NAME+"("+CALL_ROWID+"INTEGER PRIMARY KEY AUTOINCREMENT,"+CALL_NUMBER+"INTEGER "+CALL_DURATION+"INTEGER, "+CALL_TYPE+"INTEGER "+");";
    private static class DatabaseHelper extends SQLiteOpenHelper {
        public DatabaseHelper(Context context) {
            super(context,DATABASE_NAME,null,DATABASE_VERSION);
        }

        @Override
        public void onCreate(SQLiteDatabase db) {
            db.execSQL(Records);

        }

        @Override
        public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            db.execSQL("DROP TABLE IF EXIST"+Records);

        }
    }

public  CallRecords open(){
    DBhelper=new DatabaseHelper(context);
    callDB=DBhelper.getWritableDatabase();

    return this;
}
    public void close(){
        DBhelper.close();
    }
    public long insertValues(String rectable,ContentValues conValues){
        return callDB.insert(rectable,null,conValues);
    }
    public Cursor getAllLogs() {
        String sql = "SELECT CALL_ROWID,CALL_TYPE,CALL_DURATION,CALL_NUMBER FROM Records";
        Cursor cursor = callDB.rawQuery(sql, null);
        return cursor;
    }
        }
