# Azure Event Hub

## What it stands for
### Key Characteristics
- **Big data event streaming service**
- **Scalable** up to **terabytes of data** and millions of events per second
- **Reliable** with zero data loss
- Supports multiple protocols and SDKs

### When to use
Any scenario to analyze stream of data:
- Anomaly detection (fraud/outliers)
- Application logging
- Analytics pipelines (such as clickstreams)
- Live dashboarding
- Archiving data
- Transaction processing
- User telemetry processing
- Device telemetry streaming

## Basics

### Event producers
Event producers **send** events via AMQP, HTTP or Apache Kafka
### Event hub
Event hub has **partitions** (1 to 32).  
Once producers start sending events, **load balancer** distributes these events between all partitions (partitions grow in different rate).  
Each partition is **ordered**.  
**Partition key** allows for ordered processing. In that case events are going for single partition.  
### Namespace
**Each event hub** represent **unique stream of data**.  
Event hub namespace is a collection of event hubs:
- Scoping container
- Shared properties





