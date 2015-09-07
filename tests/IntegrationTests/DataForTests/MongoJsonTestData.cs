using System;

namespace IntegrationTests.DataForTests
{
    public class TestData
    {
        public string Id;
        public string EntityId;
        public string Json;
        public string ParentApiId;

        public TestData(string id, string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException();

            Json = json;

            if (string.IsNullOrEmpty(id))
                EntityId = FindPropertyValue("id");
            else
                EntityId = id;

            Id = "testId" + EntityId;
            Json = Json.Replace("{0}", Id); // fix: string format doesn't work with json
            Json = Json.Replace("{1}", EntityId); // fix: string format doesn't work with json

            ParentApiId = FindPropertyValue("parent_api__id");
        }

        private string FindPropertyValue(string propertyKey)
        {
            string resultValue = string.Empty;

            string[] splitedArray = Json.Split(',');

            foreach (var item in splitedArray)
            {
                if (item.Contains(string.Format("'{0}'", propertyKey)))
                {
                    int positionToValuePlacement = item.LastIndexOf(':');
                    resultValue = item.Remove(0, positionToValuePlacement + 1).Trim();
                    resultValue = resultValue.Replace(@"'", "");
                    break;
                }

            }

            return resultValue;
        }
    }

    public static class MongoJsonTestData
    {

        #region Season
        public static TestData Season = new TestData("http://api.sportsdatallc.org/nfl-t1/2014/REG/schedule.xml", @"
        {
            '_id' : '{0}',    
            'id' : '{1}',   
            'xmlns' : 'http://feed.elasticstats.com/schema/nfl/schedule-v1.0.xsd',
            'parent_api__id' : 'schedule',
            'dd_updated__id' : 1432330350488
            'season' : 2014.0000000000000000,
            'season_type' : 'PRE',
            'team' : 'NO',
            'type' : 'REG',
            'weeks' : [ 
                {
                    'week' : {
                        'week' : 1.0000000000000000,
                        'games' : [ 
                            {
                                'game' : '3c42f4ea-e4b3-449d-82d5-36850144add9'
                            }, 
                            {
                                'game' : '269f5756-9bc0-47b6-a4d1-97534e9ab7c1'
                            }                
                        ]
                    }
                },         
                {
                    'week' : {
                        'week' : 16.0000000000000000,
                        'games' : [ 
                            {
                                'game' : '60d838ed-dbbc-47ae-95f5-199d552974b6'
                            }, 
                            {
                                'game' : 'add46776-4ab6-4662-9c98-e86919bcfae9'
                            }
                        ]
                    }
                }, 
                {
                    'week' : {
                        'week' : 17.0000000000000000,
                        'games' : [ 
                            {
                                'game' : '3ffec59f-a194-4bc8-ae6d-2673d1714f6a'
                            }, 
                            {
                                'game' : '3ef96008-4405-42d4-854a-96141d3cc735'
                            }
                        ]
                    }
                }
            ]
        }
        ");


        #endregion Season

        #region Game


        public static TestData Game = new TestData("3c42f4ea-e4b3-449d-82d5-36850144add9", @"
            {
                '_id' : '{0}',
                'away' : 'GB',
                'away_rotation' : '',
                'home' : 'SEA',
                'home_rotation' : '',
                'id' : '{1}',
                'scheduled' : '2014-09-05T00:30:00+00:00',
                'status' : 'closed',
                'parent_api__id' : 'schedule',
                'dd_updated__id' : 1432330350488,
                'season__id' : 'http://api.sportsdatallc.org/nfl-t1/2014/REG/schedule.xml',
                'venue' : 'c6b9e5df-c9e4-434c-b3e6-83928f11cbda',
                'weather__list' : {
                    'condition' : 'Sunny',
                    'humidity' : 50.0000000000000000,
                    'temperature' : 73.0000000000000000,
                    'wind__list' : {
                        'direction' : 'WNW',
                        'speed' : 11.0000000000000000
                    }
                },
                'broadcast__list' : {
                    'cable' : '',
                    'internet' : '',
                    'network' : 'NBC',
                    'satellite' : ''
                },
                'links__list' : [ 
                    {
                        'link' : {
                            'href' : '/2014/REG/1/GB/SEA/statistics.xml',
                            'rel' : 'statistics',
                            'type' : 'application/xml'
                        }
                    }, 
                    {
                        'link' : {
                            'href' : '/2014/REG/1/GB/SEA/summary.xml',
                            'rel' : 'summary',
                            'type' : 'application/xml'
                        }
                    }
                ]
            }
            ");
        #endregion Game

        #region Play

        public static TestData Play = new TestData(string.Empty, @"
          {
            '_id' : '{0}',
            'clock' : '15:00',
            'down' : 1.0000000000000000,
            'id' : '86d7d20c-e031-449d-bf60-3ebe623123b1',
            'sequence' : 2.0000000000000000,
            'side' : 'SEA',
            'type' : 'kick',
            'updated' : '2014-09-05T00:42:18+00:00',
            'yard_line' : 35.0000000000000000,
            'yfd' : 10.0000000000000000,
            'parent_api__id' : 'pbp',
            'dd_updated__id' : NumberLong(1434052433252),
            'game__id' : '3c42f4ea-e4b3-449d-82d5-36850144add9',
            'participants__list' : [ 
                {
                    'player' : '40cda44b-2ee3-4ad1-834e-995e30db84d4'
                }, 
                {
                    'player' : '7ea2fcfd-0099-4e62-8f6e-efa0197bbb99'
                }, 
                {
                    'player' : 'c4e604ea-cea1-4cde-bdb0-e95a747db0ee'
                }
            ],
            'summary' : '4-S.Hauschka kicks 71 yards from SEA 35. 26-D.Harris to GB 13 for 19 yards (57-M.Morgan).',
            'links__list' : {
                'link__list' : {
                    'href' : '/2014/REG/1/GB/SEA/plays/86d7d20c-e031-449d-bf60-3ebe623123b1.xml',
                    'rel' : 'summary',
                    'type' : 'application/xml'
                }
            }
        }
            ");
        #endregion Play

        #region Team

        public static TestData Team = new TestData("BUF", @"
            {
                '_id' : '{0}',
                'id' : '{1}',
                'market' : 'Buffalo',
                'name' : 'Bills',
                'parent_api__id' : 'hierarchy',
                'dd_updated__id' : NumberLong(1432330620462),
                'league__id' : 'NFL',
                'conference__id' : 'AFC',
                'division__id' : 'AFC_EAST',
                'venue' : 'e9e0828e-37fc-4238-a317-49037577dd55'
            }

            ");
        #endregion Team

        #region Conference
        public static TestData Conference = new TestData("AFC", @"
            {
                '_id' : '{0}',
                'id' : '{1}',
                'name' : 'AFC',
                'parent_api__id' : 'hierarchy',
                'dd_updated__id' : NumberLong(1432330620462),
                'league__id' : 'NFL',
                'divisions' : [ 
                    {
                        'division' : 'AFC_EAST'
                    }, 
                    {
                        'division' : 'AFC_NORTH'
                    }, 
                    {
                        'division' : 'AFC_SOUTH'
                    }, 
                    {
                        'division' : 'AFC_WEST'
                    }
                ]
            }

            ");

        #endregion Conference

        #region Division
        public static TestData Division = new TestData("AFC_EAST", @"
            {
                '_id' : '{0}',
                'id' : '{1}',
                'name' : 'AFC East',
                'parent_api__id' : 'hierarchy',
                'dd_updated__id' : NumberLong(1432330620462),
                'league__id' : 'NFL',
                'conference__id' : 'AFC',
                'teams' : [ 
                    {
                        'team' : 'BUF'
                    }, 
                    {
                        'team' : 'MIA'
                    }, 
                    {
                        'team' : 'NYJ'
                    }, 
                    {
                        'team' : 'NE'
                    }
                ]
            }
            ");

        #endregion Division

        #region Player

        public static TestData Player1 = new TestData("64d9a11b-2d05-4173-ac72-4f9e63fb4aa6", @"
            {
                '_id' : '{0}',
                'id' : '{1}',
                'jersey' : 28.0000000000000000,
                'name' : 'C.J. Spiller',
                'position' : 'RB',
                'team' : 'BUF',
                'parent_api__id' : 'pbp',
                'dd_updated__id' : NumberLong(1432824161195),
                'game__id' : 'eb3bb333-6ae5-417c-b9e3-1d3dfdb8673e',
                'play__id' : 'b6ded994-556f-4221-af6d-ef925055f698',
                'parent_list__id' : 'participants__list'
            }
            ");


        public static TestData Player2 = new TestData(string.Empty, @"
            {
                '_id' : '{0}',
                'depth' : 1.0000000000000000,
                'id' : '5f3cc875-e802-46b2-81ad-3ffb7a3a1662',
                'jersey_number' : 1.0000000000000000,
                'name_full' : 'Michael Palardy',
                'position' : 'K',
                'status' : 'ACT',
                'parent_api__id' : 'depthchart',
                'dd_updated__id' : NumberLong(1432328610564),
                'team__id' : 'STL',
                'parent_list__id' : 'special_teams__list'
            }
            ");

        public static TestData PlayerPartisipant1 = new TestData(string.Empty, @"
            {
                '_id' : '{0}',
                'id' : '40cda44b-2ee3-4ad1-834e-995e30db84d4',
                'jersey' : 4.0000000000000000,
                'name' : 'Steven Hauschka',
                'position' : 'K',
                'team' : 'SEA',
                'parent_api__id' : 'pbp',
                'dd_updated__id' : NumberLong(1434052433252),
                'game__id' : '3c42f4ea-e4b3-449d-82d5-36850144add9',
                'play__id' : '86d7d20c-e031-449d-bf60-3ebe623123b1',
                'parent_list__id' : 'participants__list'
            }
            ");
        public static TestData PlayerPartisipant2 = new TestData(string.Empty, @"
            {
                '_id' : '{0}',
                'id' : '7ea2fcfd-0099-4e62-8f6e-efa0197bbb99',
                'jersey' : 26.0000000000000000,
                'name' : 'DuJuan Harris',
                'position' : 'RB',
                'team' : 'GB',
                'parent_api__id' : 'pbp',
                'dd_updated__id' : NumberLong(1434052433252),
                'game__id' : '3c42f4ea-e4b3-449d-82d5-36850144add9',
                'play__id' : '86d7d20c-e031-449d-bf60-3ebe623123b1',
                'parent_list__id' : 'participants__list'
            }
            ");

        #endregion Player
    }
}
