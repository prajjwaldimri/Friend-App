package com.example.admin.friend;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources;
import android.graphics.Typeface;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.ogaclejapan.smarttablayout.SmartTabLayout;
import com.ogaclejapan.smarttablayout.utils.v4.FragmentPagerItemAdapter;
import com.ogaclejapan.smarttablayout.utils.v4.FragmentPagerItems;

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

        SettingPageFragment settingPageFragment = new SettingPageFragment();
        settingPageFragment._themeShared = PreferenceManager.getDefaultSharedPreferences(getApplicationContext());
        settingPageFragment._themecolor = settingPageFragment._themeShared.toString();
        if (settingPageFragment._themecolor.equals(settingPageFragment._themeShared)) {
            getApplication().setTheme(Integer.parseInt(settingPageFragment._themecolor));

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
            SmartTabLayout viewPagerTab = (SmartTabLayout) findViewById(R.id.smarttabs);
            viewPagerTab.setViewPager(viewPager);
            TextView tx = (TextView) findViewById(R.id.toolbarTextView);
            Typeface custom_font = Typeface.createFromAsset(getAssets(), "UBUNTU-R.TTF");
            tx.setTypeface(custom_font);
        }
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




