# noah
DoSing and flooding tool for educational and entirely legal purposes only.

## How it works
Attacks with noah (and DoS/DDoS in general) work by overwhelming a
server with so many packets it can no longer function right. Noah gives
you the ability to control how many packets you send to a server with
thread delays, time limits, and thread limits. These are command line
flags that can be set from the console.

## Using Noah

### Setting the Host Name
In almost every instance that noah is ran, you will need to set the
hostname of where you wish to attack. This is the internet address of
the target server. This can either be in the format of an IP or a
hostname. It is important to note that the hostname should be in the
format of *.* or www.*.* and not http://*.* or something else.

To set the hostname, use the command line flag -n or --host-name [Host]
to set this. For example:
```
./Noah.exe --host-name www.github.com
```

### Setting the Port
DoS/DDoS are mainly used on web servers, which generally operate on
port 80, however there may be reasons why you would want to change
the port that noah attacks on.

To change the port use the -p --port [Port] flag. For example:
```
./Noah.exe --host-name www.github.com --port 8080
```

### Set the Attack Type
Noah currently has two attack modes, Tcp and Udp, with a default of
Tcp.

To change the attack mode use the -a --attack-type [Protocol] flag. For example:
```
./Noah.exe --host-name www.github.com --attack-type Udp
```

### Slowing Down the Attack
For whatever reasons that might include computer load or testing, you
may wish to slow down the rate that noah sends a packet. This will cause
the thread to sleep for a set amount of milliseconds before sending
another packet.

To cause a delay use the -d --delay [Milliseconds] flag. For example:
```
./Noah.exe --host-name www.github.com --delay 5
```

This example will cause there to be a 5 millisecond delay in between
packets being sent.

### Limiting the Time
If you wish to only conduct the attack for a certain amount of time
before shutting down, noah can limit it's execution time from the default
of infinite.

To set a time limit for the attack, use the -l --time-limit [Seconds] flag. For example:
```
./Noah.exe --host-name www.github.com --time-limit 15
```

This example will cause the attack to only last 15 seconds.

### Increasing the Attack Speed
To increase the attack speed of noah (and the amount of packets sent), you
can abuse a feature of modern computers called threading, where multiple
threads on the Operating System will all attack together. By default, noah
just uses a single thread to conduct it's attacks, but as many as hundreds
of threads can be used (though you want to be safe with this) for the
attack.

To set the amount of threads attacking, use the -t --threads [ThreadLimit] flag. For example:
```
./Noah.exe --host-name www.github.com --threads 20
```

### Seeing Execution Time
To view a counter for each second Noah is executing, as well as an
ending time result once the attack is complete, you can set a mode
to show you this information.

To turn show time mode on, use the -s --show-time switch. For example:
```
./Noah.exe --host-name www.github.com --show-time
```

### Seeing Packet Count
You might wish to see a count of how many packets were sent once
the attack is over. This would usually be coupled with a --time-limit
flag as otherwise the program would execute infinitely and the output
would never be seen.

To see the packet count, use the -f --show-flood switch. For example:
```
./Noah.exe --host-name www.github.com --time-limit 5 --show-flood
```

This would output the number of packets sent to github.com after the
5 second attack.
