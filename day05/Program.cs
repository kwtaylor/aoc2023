using System.Text.RegularExpressions;

var input = new StreamReader("input");

var seeds = new LinkedList<long>();

foreach(var seed in input.ReadLine()?.Split(':')[1].Split(' ',StringSplitOptions.RemoveEmptyEntries)) {
    seeds.AddLast(long.Parse(seed));
}

var destmap = new LinkedList<long>();

string cat;
string line;
while((line = input.ReadLine()) != null) {
    var cat_match = Regex.Match(line, @"\S+-to-(\S+)");
    if (cat_match.Success) {
        //add leftover non-mapped values (this is redudant the 1st time through but oh well)
        Console.WriteLine("Leftover: " + string.Join(", ", seeds));
        foreach(var seed in seeds) {
            destmap.AddLast(seed);
        }

        Console.WriteLine(string.Join(", ", destmap));

        cat = cat_match.Groups[1].Value;
        Console.WriteLine($" category: {cat}");

        seeds = destmap;
        destmap = new LinkedList<long>();

        continue;
    }

    var map = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (map.Length < 3) continue;

    var dest_start = long.Parse(map[0]);
    var src_start = long.Parse(map[1]);
    var inc = long.Parse(map[2]);

    // // remove and map any values that fall into range (Part 1 version)
    // var cur = seeds.First;
    // while(cur != null) {
    //     var next = cur.Next;
    //     var val = cur.Value;
    //     if(val >= src_start && val < src_start+inc) {
    //         destmap.AddLast(val - src_start + dest_start);
    //         seeds.Remove(cur);
    //     }
    //     cur = next;
    // }

    // (Part 2 version)
    var cur = seeds.First;
    while(cur != null) {
        var rng = cur.Next;
        var next = rng.Next;
        var val = cur.Value;
        var range = rng.Value;

        // is any of range in map?
        if(src_start+inc > val && src_start < val+range) {
            Console.WriteLine($"Inrange: {src_start}-{src_start+inc-1}, {val}-{val+range-1}");
            var map_val = val;
            var map_range = range;
            // if start of range isn't in map, split off
            if(map_val < src_start) {
                var split_low = map_val;
                var split_high = src_start;
                seeds.AddLast(split_low);
                seeds.AddLast(split_high - split_low);
                Console.WriteLine($"**SPLIT_L: {split_low}-{split_high-1}");
                map_range = range - (split_high-split_low);
                map_val = split_high;
            }
            // if end of range isn't in map, split off
            if(map_val+map_range > src_start+inc) {
                var split_low = src_start+inc;
                var split_high = map_val+map_range;
                seeds.AddLast(split_low);
                seeds.AddLast(split_high - split_low);
                Console.WriteLine($"**SPLIT_R: {split_low}-{split_high-1}");
                map_range = split_low - map_val;
            }
            // add mapped range to dest
            var dest_val = map_val-src_start+dest_start;
            destmap.AddLast(dest_val);
            destmap.AddLast(map_range);
            Console.WriteLine($"**MAPPED: {map_val}-{map_val+map_range-1} to {dest_val}-{dest_val+map_range-1}");
            // get possibly updated next pointer
            next = rng.Next;
            // remove mapped ranges from source
            seeds.Remove(cur);
            seeds.Remove(rng);
        }
        cur = next;
    }
}

//add leftover non-mapped values  (probably could avoid copy paste but oh well)
Console.WriteLine("Leftover: " + string.Join(", ", seeds));
foreach(var seed in seeds) {
    destmap.AddLast(seed);
}

Console.WriteLine(string.Join(", ", destmap));
// // part 1 just get min
// Console.WriteLine(destmap.Min());
// part 2 get min of starting locations
long minpos = long.MaxValue;
var mincur = destmap.First;
while(mincur != null) {
    minpos = long.Min(minpos, mincur.Value);
    mincur = mincur.Next.Next;
}

Console.WriteLine(minpos);
