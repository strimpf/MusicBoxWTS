# MusicBoxWTS
Music Box Sales and Special Orders application initially created as Masters Degree project

Created the project as a Universal Windows Project (UWP) because I thought it would be cool to do.

This was my first foray into the world of XAML though I have done quite a bit of C# web development in the past.

I plan to rewrite the application as a MVVC application.  The main reason for this is that I didn't realize that UWP apps are meant to
be published to the Microsoft Store and installed from there.  This application uses a MySQL database and was meant to be installed 
on one laptop.  This became problematic when I tried to deploy the application to the laptop.  I had to setup the laptop with a Developers'
License and Sideload the application, and by the way, had to run the "checknetisolation loopbackexempt -a -n=CE0E3A.... command in an 
Administrator cmd window in order for the application to reach the database.  In all, I spent about 3 days trying to figure out how to 
build and deploy the application.


