XG Management Helper
By: Alan Toews (alan@toews.xyz)
Created May 4, 2020
==============================

WARNING:
Every effort was made to ensure this application will operate with minimal risk to the user. However, 
changing the named admin password through an automated solution carries some risk. The application attempts 
to account for any possible failures present both the results of the attempt, and the password used in the
attempt, but if any unforseen disruption prevents the application from successfully recording and displaying 
the pasword used, then you may be left with no record of the admin password used. Admin account passwords 
can be reset with physical access to the system, during a reboot of hte appliance. However, the safest and
easiest recovery method is to ensure the firewall is manged by Sophos Central. This will allow you to easily reset
the admin password even if unforseen events occur. 

Prerequisites:
This application works over SSH port 22 to access and manage selected firewalls. It requires that SSH be accessible form the location that this application is running from. While SSH being accessible widely to the internet is strongly discouraged, XG does offer some tools to allow remote SSH access to selected firewalls. 

Features:
'admin' account password management
It can be challenging to update admin passwords on a many systems. This tool helps to automate

