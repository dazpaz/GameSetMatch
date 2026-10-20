import { test as bddTest } from 'playwright-bdd';
import { TournamentListPage } from './drivers/tournament-list-page';

export const test = bddTest.extend<{ tournamentListPage: TournamentListPage }>({
  tournamentListPage: async ({ page }, use) => {
    await use(new TournamentListPage(page));
  },
});
