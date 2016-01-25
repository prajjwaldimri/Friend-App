Friend's
=========
[![Windows Build status](https://ci.appveyor.com/api/projects/status/cie691osqna780e4/branch/master?svg=true)](https://ci.appveyor.com/project/prajjwaldimri/friend-app/branch/master)
[![License APACHE](https://img.shields.io/badge/license-APACHE-642C90.svg?style=flat-square)](https://github.com/prajjwaldimri/Friend-App/blob/master/LICENSE)
[![Contact](https://img.shields.io/badge/contact-@prajjwaldimri-642C90.svg?style=flat-square)](https://twitter.com/prajjwaldimri)
[![Android Build Travis](https://img.shields.io/travis/prajjwaldimri/Friend-App.svg?style=flat-square)](https://travis-ci.org/prajjwaldimri/Friend-App)
[![GitHub issues](https://img.shields.io/github/issues/prajjwaldimri/Friend-App.svg?style=flat-square)](https://github.com/prajjwaldimri/Friend-App/issues)


The app that protects you physically!

## Requirements

#### (For Windows)
- Visual Studio 2015 
- Windows Universal SDKs
- Most dependencies are obtained from Nuget and automatically restored on build.

#### (For Android)
- Android Studio 
- API 16,23 SDK
- Google Play Services SDK


## Building

First clone the repo

    git clone https://github.com/prajjwaldimri/Friend-App

#### (In Windows)

Open the solution file `Friend's.sln` in Visual Studio.  Make sure the selected platform is either `x86` (for emulator) or `ARM` (for device).  Then Build The Solution.  Nuget should auto download all missing packages, if not open the package manager and click `Restore Missing Packages`.

#### (In Android)

Open the sub-folder named Friend1 inside the Android folder of Project directly in Android Studio. 


#### Please refer to the wiki for further information: https://github.com/prajjwaldimri/Friend-App/wiki
