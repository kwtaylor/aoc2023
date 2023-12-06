var input = new StreamReader("input");
var times = new List<int>();
var dists = new List<int>();

string time_cat = "";
string dist_cat = "";

foreach (var time in input.ReadLine()?.Split(' ',StringSplitOptions.RemoveEmptyEntries)[1..]) {
    times.Add(int.Parse(time));
    time_cat += time;
}

long bigtime = long.Parse(time_cat);

foreach (var dist in input.ReadLine()?.Split(' ',StringSplitOptions.RemoveEmptyEntries)[1..]) {
    dists.Add(int.Parse(dist));
    dist_cat += dist;
}

long bigdist = long.Parse(dist_cat);

var ways = new List<int>();

foreach (var race in times.Zip(dists, (t,d) => (t,d))) {
    int wins = 0;
    for (int t=0; t<=race.t; t++) {
        if((race.t-t)*t > race.d) wins++;
    }
    ways.Add(wins);
}

Console.WriteLine($"Part 1: {ways.Aggregate(1,(a,b) => a*b)}");

// yeah i'm just brute forcing so what
int bigwins = 0;
for (int t=0; t<=bigtime; t++) checked {
    if((bigtime-t)*t > bigdist) bigwins++;
}
Console.WriteLine($"Part 2: {bigwins}");
