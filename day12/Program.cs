using System.Reflection.Metadata.Ecma335;

long total=0;
int n=0;
var cache = new Dictionary<(string,int),long>();

foreach(var line in File.ReadLines("input")) {
    var split = line.Split(' ');
    var spr = split[0];
    var dam = split[1].Split(',').Select((s,_) => int.Parse(s)).ToArray();

    //Part 2 shenanigans
    spr = spr+"?"+spr+"?"+spr+"?"+spr+"?"+spr;
    dam = [.. dam, .. dam, .. dam, .. dam, .. dam];

    Console.WriteLine($"{n} {spr} {String.Join(',',dam)}");

    cache.Clear();
    long cnt = FillAndCheck(spr,dam);
    total+= cnt;
    Console.WriteLine($"{n} cnt {cnt} total {total}");
    n++;
}

Console.WriteLine(total);




long FillAndCheck(string spr, int[] dam) {
    long count=0;
    int dlen = dam.Length;

    if(cache.TryGetValue((spr,dlen),out count)) {
        //Console.WriteLine($"cache hit! {spr} {String.Join(' ', dam)} count {count}");
        return count;
    } else {
        //Console.WriteLine($"cache miss... {spr} {String.Join(' ', dam)}");
    }

    int pos=0;
    int len=0;
    bool has_spring = false;
    foreach(var c in spr) {
        // scan for first possible location for 1st set
        if(c == '.' && len==0) 
            pos++;
        else if(c == '.' && len>0)
            break;
        else {
            if(c=='#') has_spring=true;
            len++;
        }
        
        if(len==dam[0]) break;
    }

    if(len==dam[0]) {
        // found fit for 1st set
        if(dam.Length == 1) {
            // last set
            if(spr.IndexOf('#',pos+len) <0) {
                // no leftover springs, count the match
                count++;
            }
        } else if(spr.Length > pos+len+1 && spr[pos+len] != '#') {
            // good match, continue matching next set
            count += FillAndCheck(spr[(pos+len+1)..],dam[1..]);
        }

        if(spr.Length > pos+1 && spr[pos] != '#') {
            // try next starting point for this set
            count+=FillAndCheck(spr[(pos+1)..],dam);
        }
    
    } else if (spr.Length > pos+len+1 && !has_spring) {
        //nofit but was all ??'s, skip over
        count+=FillAndCheck(spr[(pos+len+1)..],dam);
    }

    cache[(spr,dlen)] = count;
    return count;
}

