entity some_test_bench is
end entity;

architecture some_test_bench of some_test_bench is
	signal x1, x2 : integer := 0;
	signal clk : bit := '0';
	variable clk_var : bit := '0';
begin

	compute_xor: process
	begin
		report(clk);
		clk <= not clk after 5 ns;
		--clk <= '1' after 5 ns, '0' after 15 ns, '1' after 20 ns;
		--clk <= '1' after 5 ns;
		clk_var := not clk_var;
		wait;
	end process;

end architecture some_test_bench;