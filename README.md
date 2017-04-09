KinectChipCounter
=================

A Kinect application designed to look at a poker table and count the chips.

This used to be a pain in the ass to get wired up and ready to go, but I set up the dependencies to be resolved with NuGet.

Emgu 3 is wired in, but the Kinect integration is out of date, and will likely get reworked at some point when it's time for depth sensing.

Right now, the color detection works, but very jankily.  I need to wired up something better.

Chip tracking is also very basic, and breaks VERY quickly on crappy cameras.  I need to get something better than the horrifying webcam in my current computer.

Next up . . . 

Instead of the Kinect, maybe we'll look at one of these.

https://stimulant.com/depth-sensor-shootout-2/