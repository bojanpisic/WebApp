using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }


        #region RACS user methods
        [HttpGet]
        [Route("get-toprated-racs")]
        public async Task<IActionResult> GetTopRatedRACS()
        {
            var racs = await unitOfWork.RentACarRepository.Get(null, null, "Rates");

            List<Tuple<float, RentACarService>> newList = new List<Tuple<float, RentACarService>>();
            float sum = 0;

            foreach (var item in racs)
            {
                sum = 0;
                foreach (var rate in item.Rates)
                {
                    sum += rate.Rate;
                }

                newList.Add(new Tuple<float, RentACarService>(sum == 0 ? 0 : sum /item.Rates.Count, item));
            }

            var tuples = newList.OrderBy(n => n.Item1).Take(5);

            return Ok(tuples);
        }
        [HttpGet]
        [Route("rent-car-services")]
        public async Task<IActionResult> RentCarServices()
        {
            try
            {
                var queryString = Request.Query;
                var sortType = queryString["typename"].ToString();
                var sortByName = queryString["sortbyname"].ToString();
                var sortTypeCity = queryString["typecity"].ToString();
                var sortByCity = queryString["sortbycity"].ToString();

                var services = await unitOfWork.RentACarRepository.Get(null, null, "Branches,Address,Rates");

                if (!String.IsNullOrEmpty(sortByCity) && !String.IsNullOrEmpty(sortTypeCity))
                {
                    if (sortType.Equals("ascending"))
                    {
                        services.OrderBy(a => a.Address.City);

                    }
                    else
                    {
                        services.OrderByDescending(a => a.Address.City);
                    }
                }
                if (!String.IsNullOrEmpty(sortByName) && !String.IsNullOrEmpty(sortType))
                {
                    if (sortType.Equals("ascending"))
                    {
                        services.OrderBy(a => a.Name);
                    }
                    else
                    {
                        services.OrderByDescending(a => a.Name);
                    }
                }

                List<object> retList = new List<object>();
                List<object> branches = new List<object>();

                foreach (var item in services)
                {
                    branches = new List<object>();
                    foreach (var d in item.Branches)
                    {
                        branches.Add(new
                        {
                            City = d.City,
                            State = d.State
                        });
                    }

                    var sum = 0.0;
                    foreach (var r in item.Rates)
                    {
                        sum += r.Rate;
                    }

                    float rate = sum == 0 ? 0 : (float)sum / item.Rates.ToArray().Length;

                    //var rate = 0.0;
                    //if (item.Rates.Count > 0)
                    //{
                    //    foreach (var r in item.Rates)
                    //    {
                    //        rate += r.Rate;
                    //    }

                    //    rate = rate / item.Rates.Count();
                    //}

                    retList.Add(new
                    {
                        Name = item.Name,
                        Logo = item.LogoUrl,
                        City = item.Address.City,
                        State = item.Address.State,
                        //Lon = item.Address.Lon,
                        //Lat = item.Address.Lat,
                        Rate = rate,
                        About = item.About,
                        Id = item.RentACarServiceId,
                        Branches = branches,
                        rate = rate
                    });
                }

                return Ok(retList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return RACS");
            }
        }

        [HttpGet]
        [Route("rent-car-service/{id}")]
        public async Task<IActionResult> GetRentCarService(int id)
        {
            try
            {
                var res = await unitOfWork.RentACarRepository.Get(racs => racs.RentACarServiceId == id, null, "Branches,Address,Rates");
                var rentservice = res.FirstOrDefault();

                List<object> branches = new List<object>();
                foreach (var item in rentservice.Branches)
                {
                    branches.Add(new
                    {
                        item.City,
                        BranchId = item.BranchId
                    });
                }

                var sum = 0.0;
                foreach (var r in rentservice.Rates)
                {
                    sum += r.Rate;
                }

                float rate = sum == 0 ? 0 : (float)sum / rentservice.Rates.ToArray().Length;

                object obj = new
                {
                    Name = rentservice.Name,
                    About = rentservice.About,
                    City = rentservice.Address.City,
                    State = rentservice.Address.State,
                    Lat = rentservice.Address.Lat,
                    Lon = rentservice.Address.Lon,
                    Logo = rentservice.LogoUrl,
                    Id = rentservice.RentACarServiceId,
                    Branches = branches,
                    rate = rate
                };

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return RACS");
            }
        }

        [HttpGet]
        [Route("cars")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {

                var queryString = Request.Query;
                var fromDate = queryString["dep"].ToString();
                var toDate = queryString["ret"].ToString();
                DateTime DateFrom = DateTime.Now;
                DateTime DateTo = DateTime.Now;

                if (!String.IsNullOrEmpty(fromDate))
                {
                    DateFrom = Convert.ToDateTime(fromDate);
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    DateTo = Convert.ToDateTime(toDate);
                }
                if (DateFrom > DateTo)
                {
                    return BadRequest("From date should be lower then to date");
                }

                var fromCity = queryString["from"].ToString();
                var toCity = queryString["to"].ToString();
                var carType = queryString["type"].ToString();
                var seatFrom = Int32.Parse(queryString["seatfrom"].ToString());
                var seatTo = Int32.Parse(queryString["seatto"].ToString());

                float priceFrom = 0;
                float priceTo = 3000;

                if (String.IsNullOrEmpty(queryString["minprice"].ToString()))
                {
                    float.TryParse(queryString["minprice"].ToString(), out priceFrom);
                }

                if (String.IsNullOrEmpty(queryString["maxprice"].ToString()))
                {
                    float.TryParse(queryString["maxprice"].ToString(), out priceTo);
                }

                List<int> ids = new List<int>();
                if (!String.IsNullOrEmpty(queryString["racs"].ToString()))
                {
                    foreach (var item in queryString["racs"].ToString().Split(','))
                    {
                        ids.Add(int.Parse(item));
                    }
                }

                var allCars = await unitOfWork.CarRepository.AllCars();

                List<object> objs = new List<object>();
                RentACarService rentService = new RentACarService();
                Branch branch = new Branch();
                Address2 address = new Address2();

                string name;
                byte[] logo;

                foreach (var item in allCars)
                {

                    if (item.Branch == null)
                    {
                        rentService = item.RentACarService;
                        address = rentService.Address;
                        logo = rentService.LogoUrl;
                        name = rentService.Name;
                    }
                    else
                    {
                        branch = item.Branch;
                        address.City = branch.City;
                        address.State = branch.State;
                        logo = branch.RentACarService.LogoUrl;
                        name = branch.RentACarService.Name;
                    }

                    if (!FilterPass(item, ids, priceFrom, priceTo,
                        fromCity, toCity, carType, DateFrom.Date, DateTo.Date,
                        seatFrom, seatTo))
                    {
                        continue;
                    }

                    var sum = 0.0;
                    foreach (var r in item.Rates)
                    {
                        sum += r.Rate;
                    }

                    float rate = sum == 0 ? 0 : (float)sum / item.Rates.ToArray().Length;

                    objs.Add(new
                    {

                        item.CarId,
                        item.ImageUrl,
                        item.Model,
                        item.PricePerDay,
                        item.SeatsNumber,
                        item.Type,
                        item.Year,
                        item.Brand,
                        address.City,
                        address.State,
                        logo,
                        name,
                        rate = rate
                    });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return cars");
            }
        }

        [HttpGet]
        [Route("car/{id}")]
        public async Task<IActionResult> Car(int id)
        {
            try
            {
                var res = await unitOfWork.CarRepository.Get(c => c.CarId == id, null, "Branch,RentACarService,Rates");
                var car = res.FirstOrDefault();

                var sum = 0.0;
                foreach (var r in car.Rates)
                {
                    sum += r.Rate;
                }

                float rate = sum == 0 ? 0 : (float)sum / car.Rates.ToArray().Length;

                var obj = new
                {
                    car.CarId,
                    car.ImageUrl,
                    car.Model,
                    car.PricePerDay,
                    car.SeatsNumber,
                    car.Type,
                    car.Year,
                    car.Brand,
                    Name = car.Branch == null ? car.RentACarService.Name : car.Branch.RentACarService.Name,
                    Logo = car.Branch == null ? car.RentACarService.LogoUrl : car.Branch.RentACarService.LogoUrl,
                };

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return car");
            }
        }

        [HttpGet]
        [Route("racs-specialoffers/{id}")]
        public async Task<IActionResult> RacsSpecialOffers(int id)
        {
            try
            {
                var specOffers = await unitOfWork.RACSSpecialOfferRepository.GetSpecialOffersOfRacs(id);

                List<object> objs = new List<object>();

                foreach (var item in specOffers)
                {
                    objs.Add(new
                    {
                        Name = item.Car.Branch == null ? item.Car.RentACarService.Name : item.Car.Branch.RentACarService.Name,
                        Logo = item.Car.Branch == null ? item.Car.RentACarService.LogoUrl : item.Car.Branch.RentACarService.LogoUrl,
                        item.NewPrice,
                        item.OldPrice,
                        FromDate = item.FromDate.Date,
                        ToDate = item.ToDate.Date,
                        item.Car.Brand,
                        item.Car.CarId,
                        item.Car.ImageUrl,
                        item.Car.Model,
                        item.Car.SeatsNumber,
                        item.Car.Type,
                        item.Car.Year
                    });
                }

                return Ok(objs);

            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return special offers");
            }
        }

        private bool FilterPass(Car item, List<int> ids, float priceFrom, float priceTo, string fromCity,
            string toCity, string carType, DateTime from, DateTime to, int seatFrom, int seatTo)
        {
            int numOfDays = Math.Abs(from.Day - to.Day);
            if (ids.Count > 0)
            {
                if (item.RentACarService != null)
                {
                    if (!ids.Contains(item.RentACarService.RentACarServiceId))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!ids.Contains(item.Branch.RentACarServiceId))
                    {
                        return false;
                    }
                }

            }
            if (item.PricePerDay * numOfDays < priceFrom || item.PricePerDay * numOfDays > priceTo)
            {
                return false;
            }

            //rezervacije proveriti da li je slobodan
            if (!item.Type.Equals(carType) && carType != "")
            {
                return false;
            }
            RentACarService racs = null;
            if (item.RentACarService != null)
            {
                racs = item.RentACarService;
            }
            else
            {
                racs = item.Branch.RentACarService;
            }



            foreach (var offer in item.SpecialOffers)
            {
                if (offer.FromDate.Date >= from && offer.FromDate <= to || offer.ToDate.Date >= from && offer.ToDate <= to)
                {
                    return false;
                }
            }

            if (item.SeatsNumber < seatFrom || item.SeatsNumber > seatTo)
            {
                return false;
            }
            bool fromFound = false;
            bool toFound = false;

            if (racs.Address.City == fromCity)
            {
                fromFound = true;
            }
            if (racs.Address.City == toCity)
            {
                toFound = true;
            }

            foreach (var branch in racs.Branches)
            {
                if (branch.City == fromCity && !fromFound)
                {
                    fromFound = true;
                }
                if (branch.City == toCity && !toFound)
                {
                    toFound = true;
                }
            }

            if (!toFound || !fromFound)
            {
                return false;
            }


            return true;
        }
        #endregion


        #region Airline user methods
        // user methods

        [HttpGet]
        [Route("get-toprated-airlines")]
        //public async Task<ActionResult<IEnumerable<Airline>>> GetTopRated() 
        public async Task<IActionResult> GetTopRated()
        {
            var airlines = await unitOfWork.AirlineRepository.Get(null, null, "Rates");

            List<Tuple<float, Airline>> newList = new List<Tuple<float, Airline>>();
            float sum = 0;

            foreach (var item in airlines)
            {
                sum = 0;
                foreach (var rate in item.Rates)
                {
                    sum += rate.Rate;
                }

                newList.Add(new Tuple<float, Airline>(sum == 0 ? 0 : sum / item.Rates.Count, item));
            }

            var tuples = newList.OrderBy(n => n.Item1).Take(5);

            return Ok(tuples);
        }

        [HttpGet]
        [Route("all-airlines")]
        public async Task<IActionResult> AllAirlinesForUser()
        {
            try
            {
                var queryString = Request.Query;
                var sortType = queryString["typename"].ToString();
                var sortByName = queryString["sortbyname"].ToString();
                var sortTypeCity = queryString["typecity"].ToString();
                var sortByCity = queryString["sortbycity"].ToString();

                var airlines = await unitOfWork.AirlineRepository.GetAllAirlines();


                if (!String.IsNullOrEmpty(sortByCity) && !String.IsNullOrEmpty(sortTypeCity))
                {
                    if (sortType.Equals("ascending"))
                    {
                        airlines.OrderBy(a => a.Address.City);

                    }
                    else
                    {
                        airlines.OrderByDescending(a => a.Address.City);
                    }
                }
                if (!String.IsNullOrEmpty(sortByName) && !String.IsNullOrEmpty(sortType))
                {
                    if (sortType.Equals("ascending"))
                    {
                        airlines.OrderBy(a => a.Name);
                    }
                    else
                    {
                        airlines.OrderByDescending(a => a.Name);
                    }
                }

                List<object> all = new List<object>();
                List<object> allDest = new List<object>();

                foreach (var item in airlines)
                {
                    allDest = new List<object>();

                    foreach (var dest in item.Destinations)
                    {
                        allDest.Add(new
                        {
                            dest.Destination.City,
                            dest.Destination.State
                        });
                    }
                    var sum = 0.0;
                    foreach (var r in item.Rates)
                    {
                        sum += r.Rate;
                    }

                    float rate = sum == 0 ? 0 : (float)sum / item.Rates.ToArray().Length;
                    //var rate = 0.0;
                    //if (item.Rates.Count > 0)
                    //{
                    //    foreach (var r in item.Rates)
                    //    {
                    //        rate += r.Rate;
                    //    }

                    //    rate = rate / item.Rates.Count();
                    //}

                    all.Add(new
                    {
                        AirlineId = item.AirlineId,
                        City = item.Address.City,
                        State = item.Address.State,
                        //Lat = item.Address.Lat,
                        //Lon = item.Address.Lon,
                        rate = rate,
                        Name = item.Name,
                        Logo = item.LogoUrl,
                        About = item.PromoDescription,
                        Destinations = allDest
                    });
                }

                return Ok(all);
            }
            catch (Exception)
            {

                return StatusCode(500, "Failed to return airlines");
            }
        }

        [HttpGet]
        [Route("airline/{id}")]
        public async Task<IActionResult> Airline(int id)
        {
            try
            {
                var airline = await unitOfWork.AirlineRepository.GetAirline(id);

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                List<object> allDest = new List<object>();


                allDest = new List<object>();

                foreach (var dest in airline.Destinations)
                {
                    allDest.Add(new
                    {
                        dest.Destination.City,
                        dest.Destination.State,
                        dest.Destination.ImageUrl
                    });
                }

                var sum = 0.0;
                foreach (var r in airline.Rates)
                {
                    sum += r.Rate;
                }

                float rate = sum == 0 ? 0 : (float)sum / airline.Rates.ToArray().Length;

                object obj = new
                {
                    AirlineId = airline.AirlineId,
                    City = airline.Address.City,
                    State = airline.Address.State,
                    Lat = airline.Address.Lat,
                    Lon = airline.Address.Lon,
                    Name = airline.Name,
                    Logo = airline.LogoUrl,
                    About = airline.PromoDescription,
                    Destinations = allDest,
                    rate = rate
                };

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return airline");
            }
        }

        [HttpGet]
        [Route("airline-special-offers/{id}")]
        public async Task<IActionResult> AirlineSpecialOffers(int id)
        {
            try
            {
                var airline = await unitOfWork.AirlineRepository.GetByID(id);

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                var offers = await unitOfWork.SpecialOfferRepository.GetSpecialOffersOfAirline(airline);

                List<object> objs = new List<object>();
                List<object> flights = new List<object>();
                List<object> fstops = new List<object>();


                foreach (var item in offers)
                {
                    flights = new List<object>();

                    foreach (var seat in item.Seats)
                    {
                        fstops = new List<object>();

                        foreach (var stop in seat.Flight.Stops)
                        {
                            fstops.Add(new
                            {
                                city = stop.Destination.City
                            });
                        }

                        flights.Add(new
                        {
                            flightId = seat.Flight.FlightId,
                            flightNumber = seat.Flight.FlightNumber,
                            to = seat.Flight.To.City,
                            from = seat.Flight.From.City,
                            tripLength = seat.Flight.tripLength,
                            tripTime = seat.Flight.TripTime,
                            stops = fstops,
                            landingDate = seat.Flight.LandingDateTime.Date,
                            landingTime = seat.Flight.LandingDateTime.TimeOfDay,
                            takeOffDate = seat.Flight.TakeOffDateTime.Date,
                            takeOffTime = seat.Flight.TakeOffDateTime.TimeOfDay,
                            seat.Class,
                            seat.Column,
                            seat.Row,
                            seat.Price,
                            seat.Reserved,
                            seat.SeatId
                        }
                        );
                    }

                    objs.Add(new { airline.LogoUrl, airline.Name, item.NewPrice, item.OldPrice, item.SpecialOfferId, flights });
                }

                return Ok(objs);

            }
            catch (Exception)
            {

                return StatusCode(500, "Failed to return special offers");
            }
        }

        [HttpGet]
        [Route("flights")]
        public async Task<IActionResult> Flights()
        {
            try
            {
                var queryString = Request.Query;
                var tripType = queryString["type"].ToString();
                var y = queryString["dep"].ToString();
                var t = queryString["dep"].ToString().Split(',');

                List<DateTime> departures = new List<DateTime>();
                if (!String.IsNullOrEmpty(queryString["dep"].ToString()))
                {
                    foreach (var item in queryString["dep"].ToString().Split(','))
                    {
                        departures.Add(Convert.ToDateTime(item));
                    }
                }

                List<string> fromList = new List<string>();
                List<string> toList = new List<string>();
                string from = "", to = "";

                if (tripType == "multi")
                {
                    foreach (var item in queryString["from"].ToString().Split(','))
                    {
                        fromList.Add(item);
                    }
                    foreach (var item in queryString["to"].ToString().Split(','))
                    {
                        toList.Add(item);
                    }
                }
                else
                {
                    from = queryString["from"].ToString();
                    to = queryString["to"].ToString();
                }


                DateTime ret = DateTime.MinValue;
                if (!String.IsNullOrEmpty(queryString["ret"].ToString()))
                {
                    ret = Convert.ToDateTime(queryString["ret"].ToString());
                }
                float minPrice = 0;
                float maxPrice = 3000;

                float.TryParse(queryString["minprice"], out minPrice);
                float.TryParse(queryString["maxprice"], out maxPrice);

                List<int> ids = new List<int>();
                if (!String.IsNullOrEmpty(queryString["air"].ToString()))
                {
                    foreach (var item in queryString["air"].ToString().Split(','))
                    {
                        ids.Add(int.Parse(item));
                    }
                }

                var minDuration = queryString["mind"].ToString();
                var maxDuration = queryString["maxd"].ToString();



                var flights = await unitOfWork.FlightRepository.GetAllFlightsWithAllProp();

                ICollection<object> flightsObject = new List<object>();
                ICollection<object> twoWayFlights = new List<object>();
                ICollection<object> oneWayFlights = new List<object>();

                ICollection<object> multiWayFlights = new List<object>();



                if (tripType == "one")
                {
                    foreach (var flight in flights)
                    {

                        if (!(await FilterFromPassed(flight, to, from, ids, minDuration, maxDuration, departures, minPrice, maxPrice)))
                        {
                            continue;
                        }

                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                //var s = await unitOfWork.AirlineRepository.GetDestination(stop.DestinationId);
                                //stops.Add(new { s.City });
                                stops.Add(new { stop.Destination.City, stop.Destination.State });

                            }
                        }


                        flightsObject.Add(new
                        {
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = flight.Airline.LogoUrl,
                            airlineName = flight.Airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops,
                            minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                        });
                        oneWayFlights.Add(new { flightsObject });
                    }
                    return Ok(oneWayFlights);
                }

                else if (tripType == "two")
                {
                    foreach (var flight in flights)
                    {

                        if (!(await FilterFromPassed(flight, to, from, ids, minDuration, maxDuration, departures, minPrice, maxPrice)))
                        {
                            continue;
                        }

                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                //var s = await unitOfWork.AirlineRepository.GetDestination(stop.DestinationId);
                                //stops.Add(new { s.City });
                                stops.Add(new { stop.Destination.City, stop.Destination.State });

                            }
                        }


                        flightsObject.Add(new
                        {
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = flight.Airline.LogoUrl,
                            airlineName = flight.Airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops,
                            minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                        });

                        foreach (var returnFlights in flights)
                        {
                            if (!(await FilterReturnPassed(flight, from, to, ids, minDuration, maxDuration, ret, minPrice, maxPrice)))
                            {
                                continue;
                            }

                            stops = new List<object>();

                            if (flight.Stops != null)
                            {
                                foreach (var stop in flight.Stops)
                                {
                                    //var s = await unitOfWork.AirlineRepository.GetDestination(stop.DestinationId);
                                    //stops.Add(new { s.City });
                                    stops.Add(new { stop.Destination.City, stop.Destination.State });

                                }
                            }


                            flightsObject.Add(new
                            {
                                takeOffDate = flight.TakeOffDateTime.Date,
                                landingDate = flight.LandingDateTime.Date,
                                airlineLogo = flight.Airline.LogoUrl,
                                airlineName = flight.Airline.Name,
                                from = flight.From.City,
                                to = flight.To.City,
                                takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                                landingTime = flight.TakeOffDateTime.TimeOfDay,
                                flightTime = flight.TripTime,
                                flightLength = flight.tripLength,
                                flightNumber = flight.FlightNumber,
                                flightId = flight.FlightId,
                                stops = stops,
                                minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                            });

                            twoWayFlights.Add(new { flightsObject });
                            flightsObject = new List<object>();
                            flightsObject.Add(new { flight });

                        }

                    }
                    return Ok(twoWayFlights);
                }
                else
                {
                    float currentPrice = 0;

                    foreach (var flight in flights)
                    {
                        List<string> tempFrom = fromList;
                        List<string> tempTo = toList;
                        List<DateTime> tempDepartures = departures;
                        currentPrice = 0;

                        if (!(await FilterMultiPassed(flight, fromList, toList, ids, minDuration, maxDuration, departures, minPrice, maxPrice, currentPrice)))
                        {
                            continue;
                        }

                        currentPrice += flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price;

                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                //var s = await unitOfWork.AirlineRepository.GetDestination(stop.DestinationId);
                                //stops.Add(new { s.City });
                                stops.Add(new { stop.Destination.City, stop.Destination.State });

                            }
                        }


                        flightsObject.Add(new
                        {
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = flight.Airline.LogoUrl,
                            airlineName = flight.Airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops,
                            minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                        });

                        tempFrom.Remove(flight.From.City);
                        tempTo.Remove(flight.To.City);
                        tempDepartures.Remove(flight.TakeOffDateTime.Date);

                        foreach (var returnFlights in flights)
                        {
                            if (tempFrom.Count == 0)
                            {
                                break;
                            }
                            if (!(await FilterMultiPassed(returnFlights, tempFrom, tempTo, ids, minDuration, maxDuration, tempDepartures, minPrice, maxPrice, currentPrice)))
                            {


                                continue;
                            }
                            currentPrice += returnFlights.Seats.OrderBy(s => s.Price).FirstOrDefault().Price;

                            tempFrom.Remove(returnFlights.From.City);
                            tempTo.Remove(returnFlights.To.City);
                            tempDepartures.Remove(returnFlights.TakeOffDateTime.Date);

                            stops = new List<object>();

                            if (returnFlights.Stops != null)
                            {
                                foreach (var stop in returnFlights.Stops)
                                {
                                    //var s = await unitOfWork.AirlineRepository.GetDestination(stop.DestinationId);
                                    //stops.Add(new { s.City });
                                    stops.Add(new { stop.Destination.City, stop.Destination.State });

                                }
                            }


                            flightsObject.Add(new
                            {
                                takeOffDate = returnFlights.TakeOffDateTime.Date,
                                landingDate = returnFlights.LandingDateTime.Date,
                                airlineLogo = returnFlights.Airline.LogoUrl,
                                airlineName = returnFlights.Airline.Name,
                                from = returnFlights.From.City,
                                to = returnFlights.To.City,
                                takeOffTime = returnFlights.TakeOffDateTime.TimeOfDay,
                                landingTime = returnFlights.TakeOffDateTime.TimeOfDay,
                                flightTime = returnFlights.TripTime,
                                flightLength = returnFlights.tripLength,
                                flightNumber = returnFlights.FlightNumber,
                                flightId = returnFlights.FlightId,
                                stops = stops,
                                minPrice = returnFlights.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                            });

                            multiWayFlights.Add(new { flightsObject });
                            flightsObject = new List<object>();
                            flightsObject.Add(new { flight });

                        }

                    }
                    if (multiWayFlights.Count == fromList.Count)
                    {
                        return Ok(multiWayFlights);
                    }
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return flights");
            }
        }

        [HttpGet]
        [Route("flight-seats/{id}")]
        public async Task<IActionResult> FlightSeats(int id)
        {
            try
            {
                var res = await unitOfWork.FlightRepository.Get(f => f.FlightId == id, null, "Seats");
                var flight = res.FirstOrDefault();

                if (flight == null)
                {
                    return NotFound("Flight not found");
                }

                //var seats = await unitOfWork.AirlineRepository.GetAllSeats(flight);

                List<object> obj = new List<object>();
                foreach (var item in flight.Seats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return flight seats");
            }
        }

        [HttpGet]
        [Route("all-airlines-specialoffers")]
        public async Task<IActionResult> AllSpecOffers()
        {
            try
            {
                var specOffers = await unitOfWork.SpecialOfferRepository.GetAllSpecOffers();

                List<object> objs = new List<object>();
                List<object> flights = new List<object>();
                List<object> fstops = new List<object>();


                foreach (var item in specOffers)
                {
                    flights = new List<object>();

                    foreach (var seat in item.Seats)
                    {
                        fstops = new List<object>();

                        foreach (var stop in seat.Flight.Stops)
                        {
                            fstops.Add(new
                            {
                                city = stop.Destination.City
                            });
                        }

                        flights.Add(new
                        {
                            flightId = seat.Flight.FlightId,
                            flightNumber = seat.Flight.FlightNumber,
                            to = seat.Flight.To.City,
                            from = seat.Flight.From.City,
                            airlineName = seat.Flight.Airline.Name,
                            airlineLogo = seat.Flight.Airline.LogoUrl,
                            tripLength = seat.Flight.tripLength,
                            tripTime = seat.Flight.TripTime,
                            stops = fstops,
                            landingDate = seat.Flight.LandingDateTime.Date,
                            landingTime = seat.Flight.LandingDateTime.TimeOfDay,
                            takeOffDate = seat.Flight.TakeOffDateTime.Date,
                            takeOffTime = seat.Flight.TakeOffDateTime.TimeOfDay,
                            seat.Class,
                            seat.Column,
                            seat.Row,
                            seat.Price,
                            seat.Reserved,
                            seat.SeatId
                        }
                        );
                    }

                    objs.Add(
                        new
                        {
                            item.Airline.Name,
                            item.Airline.LogoUrl,
                            item.NewPrice,
                            item.OldPrice,
                            item.SpecialOfferId,
                            flights
                        });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return special offers");
            }
        }


        private async Task<bool> FilterPassed(Flight flight, string tripType, string to, string from,
            List<string> fromMulti, List<string> toMulti, List<int> airlines, DateTime returnDate,
            string minDuration, string maxDuration, List<DateTime> departures, float minPrice, float maxPrice)
        {
            await Task.Yield();

            switch (tripType)
            {
                case "one":
                    if (!flight.From.City.Equals(from) || !flight.To.City.Equals(to))
                    {
                        return false;
                    }
                    if (airlines.Count > 0)
                    {
                        if (!airlines.Contains(flight.AirlineId))
                        {
                            return false;
                        }
                    }
                    if (departures[0].Date != flight.TakeOffDateTime.Date)
                    {
                        return false;
                    }
                    if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
                    {
                        return false;
                    }
                    if (!String.IsNullOrEmpty(minDuration) || !String.IsNullOrEmpty(maxDuration))
                    {
                        if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                            || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) < int.Parse(minDuration.Split('h', ' ', 'm')[1])
                            || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                            || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) > int.Parse(maxDuration.Split('h', ' ', 'm')[1]))
                        {
                            return false;
                        }
                    }
                    return true;

                case "two":
                    bool passed = false;
                    if (flight.From.City.Equals(from) && flight.To.City.Equals(to) && departures[0].Date == flight.TakeOffDateTime.Date)
                    {
                        passed = true;
                    }
                    if (flight.To.City.Equals(from) && flight.From.City.Equals(to) && returnDate.Date == flight.TakeOffDateTime.Date)
                    {
                        passed = true;
                    }

                    if (!passed)
                    {
                        return false;
                    }

                    if (airlines.Count > 0)
                    {
                        if (!airlines.Contains(flight.AirlineId))
                        {
                            return false;
                        }
                    }

                    if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
                    {
                        return false;
                    }
                    if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) < int.Parse(minDuration.Split('h', ' ', 'm')[1])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) > int.Parse(maxDuration.Split('h', ' ', 'm')[1]))
                    {
                        return false;
                    }

                    return true;

                case "multi":

                    bool pass = false;

                    for (int i = 0; i < fromMulti.Count; i++)
                    {
                        if (flight.From.City.Equals(fromMulti[i]) && flight.To.City.Equals(toMulti[i]) && departures[i].Date == flight.TakeOffDateTime.Date)
                        {
                            pass = true;
                            break;
                        }
                    }
                    if (!pass)
                    {
                        return false;
                    }
                    if (airlines.Count > 0)
                    {
                        if (!airlines.Contains(flight.AirlineId))
                        {
                            return false;
                        }
                    }
                    if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
                    {
                        return false;
                    }
                    if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) < int.Parse(minDuration.Split('h', ' ', 'm')[1])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) > int.Parse(maxDuration.Split('h', ' ', 'm')[1]))
                    {
                        return false;
                    }

                    return true;

                default:
                    break;
            }

            return false;
        }


        private async Task<bool> FilterFromPassed(Flight flight, string to, string from, List<int> airlines,
            string minDuration, string maxDuration, List<DateTime> departures, float minPrice, float maxPrice)
        {
            await Task.Yield();

            if (!flight.From.City.Equals(from) || !flight.To.City.Equals(to))
            {
                return false;
            }
            if (airlines.Count > 0)
            {
                if (!airlines.Contains(flight.AirlineId))
                {
                    return false;
                }
            }
            if (departures[0].Date != flight.TakeOffDateTime.Date)
            {
                return false;
            }
            if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(minDuration) || !String.IsNullOrEmpty(maxDuration))
            {
                if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) < int.Parse(minDuration.Split('h', ' ', 'm')[2])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) > int.Parse(maxDuration.Split('h', ' ', 'm')[2]))
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> FilterReturnPassed(Flight flight, string from, string to, List<int> airlines,
          string minDuration, string maxDuration, DateTime departure, float minPrice, float maxPrice)
        {
            await Task.Yield();

            if (!flight.From.City.Equals(from) || !flight.To.City.Equals(to))
            {
                return false;
            }
            if (airlines.Count > 0)
            {
                if (!airlines.Contains(flight.AirlineId))
                {
                    return false;
                }
            }
            if (departure.Date != flight.TakeOffDateTime.Date)
            {
                return false;
            }
            if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(minDuration) || !String.IsNullOrEmpty(maxDuration))
            {
                if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) < int.Parse(minDuration.Split('h', ' ', 'm')[2])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) > int.Parse(maxDuration.Split('h', ' ', 'm')[2]))
                {
                    return false;
                }
            }
            return true;
        }


        private async Task<bool> FilterMultiPassed(Flight flight, List<string> fromMulti, List<string> toMulti, List<int> airlines,
           string minDuration, string maxDuration, List<DateTime> departures, float minPrice, float maxPrice, float currentFlightsPrice)
        {
            await Task.Yield();

            if (fromMulti.Contains(flight.From.City) && toMulti.Contains(flight.To.City))
            {
            }
            else
            {
                return false;
            }

            if (!departures.Contains(flight.TakeOffDateTime.Date))
            {
                return false;
            }

            if (airlines.Count > 0)
            {
                if (!airlines.Contains(flight.AirlineId))
                {
                    return false;
                }
            }
            if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price + currentFlightsPrice
                || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price + currentFlightsPrice > maxPrice)
            {
                return false;
            }
            if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) < int.Parse(minDuration.Split('h', ' ', 'm')[2])
                || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) > int.Parse(maxDuration.Split('h', ' ', 'm')[2]))
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
