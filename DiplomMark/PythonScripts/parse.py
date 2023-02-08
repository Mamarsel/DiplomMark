import sys
from icrawler.builtin import GoogleImageCrawler
import datetime



catalog = sys.argv[1]
q = sys.argv[2]

google_crawler = GoogleImageCrawler(storage={'root_dir': catalog}, feeder_threads=1,
                    parser_threads=1, downloader_threads=4)
google_crawler.crawl(keyword=q, offset=0, max_num=1000,
                    min_size=(200,200), max_size=None)

google_crawler.crawl(
    keyword=q,
    max_num=1000,
    date_min=datetime(2014, 1, 1),
    date_max=datetime(2015, 1, 1))
google_crawler.crawl(
    keyword=q,
    max_num=1000,
    date_min=datetime(2015, 1, 1),
    date_max=datetime(2016, 1, 1))
