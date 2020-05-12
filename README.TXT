XG Management Helper
Created By: Alan Toews (alan.toews@sophos.com)
Created April 30, 2020
==============================

=== Features ===
Import firewall list
Import expects a simple text file with one firewall IP or hostname per line. If a unique password is required for each firewall, it may be added following the firewall address, separated by a space. Do not use quotes or other characters to wrap passwords. 
Example:
	192.168.1.1 thisismypassword
	192.168.1.2
	192.168.1.3 another_pa$$w0rD_with spaces in_it
In the above example, three firewalls would be imported, and passwords set for two of them.

'admin' account password management
It can be challenging to update admin passwords on a many systems. This tool helps to automate that task as safely as possible. 

Firmware version check
It will retrieve the current model, firmware version, and hotfix level reported by the firewall

Install hotfixes
If automatic hotfix installation is disabled, this app can trigger available hotfixes to download and install.

Register firewalls to Sophos Central 
This will enable Synchronized Security Heartbeat features

Enable firewall management and reporting from Sophos Central
A series of options will allow firewalls to be automatically registered to Sophos Central, and have management, backup, and reporting features enabled. You'll still need to login to Sophos Central and accept management of selected devices, and add them to firewall groups as necessary.

=== WARNING ===
Every effort was made to ensure this application will operate with minimal risk to the user. However, 
changing the named admin password through an automated solution carries some risk. The application attempts 
to account for any possible failures present both the results of the attempt, and the password used in the
attempt, but if any unforseen disruption prevents the application from successfully recording and displaying 
the pasword used, then you may be left with no record of the admin password used. Admin account passwords 
can be reset with physical access to the system, during a reboot of hte appliance. However, the safest and
easiest recovery method is to ensure the firewall is manged by Sophos Central. This will allow you to easily reset
the admin password even if unforseen events occur.

=== Security ===
This application stores passwords in encrypted form, using a unique encryption key on every installation. This should not be considered absolute security, as a malicious actor with access to the machine where the application is installed, could potentially access enough information to derive the key. Ensure that access to the machine where this application is used, is properly controlled, and adequate endpoint protection is installed to protect the data this application can handle. 

=== Prerequisites ===
This application works over SSH port 22 to access and manage selected firewalls. It requires that SSH be accessible form the location that this application is running from. While SSH being accessible widely to the internet is strongly discouraged, XG does offer some tools to allow remote SSH access to selected firewalls. 







