Create your own Game Cloud
==========================

1) Create an account at http://developer.kii.com
2) Create an application as explained in "Register an application"
following steps 1, 2, and 3 (disregard the other sections):
http://documentation.kii.com/en/starts/unity/
Choose Unity as platform for your app and the server location of
your backend.
3) Write down App Id and App Key assigned to your app as explained in
"Register an application" following step 4 (disregard the other 
sections):
http://documentation.kii.com/en/starts/unity/
4) Set keys from step 3 in your Unity project by choosing one of these
options:
  a) Go to "Kii Game Cloud" editor menu and setup your keys there
  b) Edit file Assets/Plugins/KiiConfig.txt and add your keys there
  c) Replace those keys in file Scripts/KiiAutoInitialize.cs in the
  Kii.Initialize() method directly
Important: options a) and b) will only work in Editor mode. If you want
to initialize Kii when building for a specific platform you'll have to
use c). If you're bulding for Android make sure the Stripping level in
your project settings is set to "Disabled" and that the Internet setting
is set to "Require" (not "Auto").

Want more info?
================

More demos: http://docs.kii.com/en/samples/
Game Cloud Tutorial: http://docs.kii.com/en/samples/Gamecloud-Unity/

Interested in Game Analytics?
=============================

We also offer a dedicated Unity SDK for Game Analytics which you can
download here: http://developer.kii.com/#/sdks
More info: http://documentation.kii.com/en/guides/unity/managing-analytics