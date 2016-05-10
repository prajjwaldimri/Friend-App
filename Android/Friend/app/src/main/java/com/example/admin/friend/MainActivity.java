package com.example.admin.friend;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Typeface;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.TextView;

import com.ogaclejapan.smarttablayout.SmartTabLayout;
import com.ogaclejapan.smarttablayout.utils.v4.FragmentPagerItemAdapter;
import com.ogaclejapan.smarttablayout.utils.v4.FragmentPagerItems;
import com.twitter.sdk.android.Twitter;
import com.twitter.sdk.android.core.TwitterAuthConfig;
import io.fabric.sdk.android.Fabric;
import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private Toolbar toolbar;
    private TabLayout tabLayout;
    private ViewPager viewPager;


    @Override
    protected void onCreate(Bundle savedInstanceState) {

       // Typeface custom_font = Typeface.createFromAsset(getAssets(), "fonts/TT1255M_.ttf");
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        viewPager = (ViewPager) findViewById(R.id.viewpager);
        setupViewPager(viewPager);
        tabLayout = (TabLayout) findViewById(R.id.tabs);
        tabLayout.setupWithViewPager(viewPager);
        SettingPageFragment settingPageFragment=new SettingPageFragment();
       settingPageFragment._themeShared= PreferenceManager.getDefaultSharedPreferences(getApplicationContext());
        settingPageFragment._themecolor=settingPageFragment._themeShared.toString();
        if (settingPageFragment._themecolor.equals(settingPageFragment._themeShared)){
            getApplication().setTheme(Integer.parseInt(settingPageFragment._themecolor));

        }



        for(int i=0;i<tabLayout.getTabCount();i++)
        {
            switch(i)
            {
                case 0:
                    tabLayout.getTabAt(i).setIcon(R.drawable.ic_action_name);
                    break;
                case 1:
                    tabLayout.getTabAt(i).setIcon(R.drawable.ic_action_name2);
                    break;
                case 2:
                    tabLayout.getTabAt(i).setIcon(R.drawable.ic_action_name3);
                    break;
                case 3:
                    tabLayout.getTabAt(i).setIcon(R.drawable.ic_action_name4);
                    break;
                case 4:
                    tabLayout.getTabAt(i).setIcon(R.drawable.ic_action_name5);
                    break;
                default:
                    break;
            }
        FragmentPagerItemAdapter adapter = new FragmentPagerItemAdapter(
                getSupportFragmentManager(), FragmentPagerItems.with(this)
                .add("Home", HomePagefragment.class)
                .add("Settings", SettingPageFragment.class)
                .add("Timer", TimerPageFragment.class)
                .add("Reminder", ReminderPage.class)
                .create());

        viewPager = (ViewPager) findViewById(R.id.viewpager);
        viewPager.setAdapter(adapter);

        SmartTabLayout viewPagerTab = (SmartTabLayout) findViewById(R.id.tabs);
        viewPagerTab.setViewPager(viewPager);

        adapter.addFragment(new HomePagefragment(), "Home");
        adapter.addFragment(new SettingPageFragment(), "Settings");
        adapter.addFragment(new TimerPageFragment(), "Timer");
        adapter.addFragment(new ReminderPage(), "Reminder");
        viewPager.setAdapter(adapter);

        TextView tx = (TextView)findViewById(R.id.toolbarTextView);
        Typeface custom_font = Typeface.createFromAsset(getAssets(), "UBUNTU-R.TTF");
        tx.setTypeface(custom_font);
    }


    class ViewPagerAdapter extends FragmentPagerAdapter {
        private final List<Fragment> mFragmentList = new ArrayList<>();
        private final List<String> mFragmentTitleList = new ArrayList<>();


        public ViewPagerAdapter(FragmentManager manager) {
            super(manager);
        }

        @Override
        public Fragment getItem(int position) {
            return mFragmentList.get(position);
        }

        @Override
        public int getCount() {
            return mFragmentList.size();
        }


        @Override
        public CharSequence getPageTitle(int position) {
            return mFragmentTitleList.get(position);
        }


        public void addFragment(Fragment fragment, String string) {
            mFragmentList.add(fragment);
            mFragmentTitleList.add(string);
        }
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
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
}
