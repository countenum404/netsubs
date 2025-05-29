
--filter-udp=443 --hostlist="/opt/zapret/lists/list-general.txt" --dpi-desync=fake --dpi-desync-repeats=11 --dpi-desync-fake-quic="/opt/zapret/files/fake/quic_initial_www_google_com.bin" --new 
--filter-udp=50000-50100 --filter-l7=discord,stun --dpi-desync=fake --dpi-desync-repeats=6 --new 
--filter-tcp=80 --hostlist="/opt/zapret/lists/list-general.txt" --dpi-desync=fake,fakedsplit --dpi-desync-autottl=2 --dpi-desync-fooling=md5sig --new 
--filter-tcp=443 --hostlist="/opt/zapret/lists/list-general.txt" --dpi-desync=fake,multidisorder --dpi-desync-split-pos=1,midsld --dpi-desync-repeats=11 --dpi-desync-fooling=md5sig --dpi-desync-fake-tls-mod=rnd,dupsid,sni=www.google.com --new 
--filter-udp=443 --ipset="/opt/zapret/lists/ipset-cloudflare.txt" --dpi-desync=fake --dpi-desync-repeats=11 --dpi-desync-fake-quic="/opt/zapret/files/fake/quic_initial_www_google_com.bin" --new 
--filter-tcp=80 --ipset="/opt/zapret/lists/ipset-cloudflare.txt" --dpi-desync=fake,fakedsplit --dpi-desync-autottl=2 --dpi-desync-fooling=md5sig --new 
--filter-tcp=443 --ipset="/opt/zapret/lists/ipset-cloudflare.txt" --dpi-desync=fake,multidisorder --dpi-desync-split-pos=1,midsld --dpi-desync-repeats=11 --dpi-desync-fooling=md5sig