# Azure Event Hub

## What it stands for
### Key Characteristics
- **Big data event streaming service**
- **Scalable** up to **terabytes of data** and millions of events per second
- **Reliable** with zero data loss
- Supports multiple **protocols** and **SDKs**

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
### Event producer
Event producers **send** events via AMQP, HTTP or Apache Kafka.

### Event consumer
**Consumer Group** is an unique view on event hub data (consumer groups read hub data separately) - one application relates to one consumer group and consumers are shards.  
Consumers are proceed that read event data (through AMQP 1.0 protocol).

### Event hub
Event hub has **partitions** (1 to 32 configurable). Each event has an **offset** of its position in partition. 
**Checkpointing** is a progress of offset save on client side.  
Once producers start sending events, **load balancer** distributes these events between all partitions (partitions grow in different rate).  
Each partition is **ordered**.  
**Partition key** allows for ordered processing. In that case events are going for single partition.  

### Namespace
**Each event hub** represent **unique stream of data**.  
Event hub namespace is a collection of event hubs:
- Scoping container
- Shared properties

## Features
### Event Capture
Event Capture **automatically deliver the streaming data in Event Hub** to an Azure Blob Storage or Azure Data Lake Store.  
### Auto-inflate (auto-scaling)
### Geo-disaster recovery (geo-replication with region pairing) 

# Useful links
- [Azure Event Hub Tutorial](https://youtu.be/Dc3P27BsK3E)