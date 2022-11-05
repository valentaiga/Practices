# ML.NET

## Goals
- [ ] Collect some data from internet to db (HLTV previous csgo matches)
- [ ] Build ML model from scrapped data
- [ ] Train ML for incoming matches

## How it works? 
1. Seed matches data for last 6 months to database
2. Use some method with INCOMING MATCH parameters to gain predictions
   1. Collect matches from database for last 6 months (for players from params)
   2. Train machine learning model to find best trainer option
   3. Use trained ML model to get predictions for selected match (from parameters)
3. Get {winrate}% of selected match

## Stack
- ML.NET
- PostgreSQL
- Dapper ORM

## Environment setup
If you need a local database follow instruction below:
Pull&Run PostgreSQL image in docker: `docker run --name postgres-db -e POSTGRES_PASSWORD=admin -p 5432:5432 -d postgres`

## Project setup

## Helpful Links
- [Free datasets for data science](https://www.kaggle.com/)
- [QuickStart: ML.NET Model Builder](https://youtu.be/cUqNzZwzUV0)