rustflakes
==========

A distributed, ordered ID generation service for .NET. The implementation is heavily derivative of Boundary's [flake](https://github.com/boundary/flake).

## Identifiers ## 

Identifiers are generated as 128-bit numbers:

* 64-bit timestamp as milliseconds since the dawn of time (January 1, 1970)
* 48-bit worker identifier
* 16-bit sequence number that is incremented when more than one identifier is requested in the same millisecond and reset to 0 when the clock moves forward

## Roadmap ##

* Bulk generation
* Sample service implementation

## Questions ##

### What should I use as the worker identifier? ###

I've been known to pull the MAC address of the first active ethernet adapter. It doesn't matter what you're using so long as it's guaranteed to be unique per generator. You could pull the last 6 bytes of the CPU identifier if that suited you.

While machine identity should be relatively meaningless in a distributed system, that doesn't mean we can't use an arbitrary indicator to achieve distinction between functioning nodes in a given time range. If you're afraid of MAC address spoofing, then you should be able to work something out. 

6 bytes gives you a lot of room for creativity. I suggest arbitrarily incrementing a number that you store in an S3 bucket. You could regenerate your worker identifier 281,474,976,710,656 times before you run out of unique values. Maybe this guy 

### When should I use this? ###

When you want time-based ordered IDs generated in many locations and sent to many locations.
When you can't generate sequential identifiers yourself (Windows Azure SQL Database, I'm looking at you).

### How is this different from XYZ? ###

It's probably not. I wrote this one afternoon in a hotel room because I was sick of thinking about T-SQL. A key differentiator between rustflakes and some other .NET based ID generators is that I made certain assumptions.

1 - Other people are smarter than me. I borrowed from their work.
2 - While machine identity isn't needed or desirable for some parts of a distributed system, they work great as arbitrary node identifiers. I don't make you choose a mechanism for your node identifier, but I don't foist my bad decisions on you either.
3 - There is no 3