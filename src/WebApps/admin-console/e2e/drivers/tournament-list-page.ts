import { expect, Page, Locator } from '@playwright/test';

export class TournamentListPage {
  private static readonly TOURNAMENT_LIST_URL = 'http://localhost:4200';
  private static readonly PAGE_TITLE = /AdminConsole/;
  private static readonly TITLE_TEXT = 'Welcome to the Admin Console';

  readonly page: Page;
  readonly title: Locator;
  readonly table: Locator;
  readonly tableHeaders: Locator;
  readonly tableRows: Locator;
  readonly showInactiveOption: Locator;
  readonly searchBox: Locator;

  constructor(page: Page) {
    this.page = page;
    this.table = this.page.locator('table');
    this.title = this.page.locator('h1');
    this.tableHeaders = this.table.locator('thead th');
    this.tableRows = this.table.locator('tbody tr:has(th[scope="row"])');
    this.showInactiveOption = this.page.getByRole('switch', { name: 'Show inactive tournaments' });
    this.searchBox = this.page.getByRole('searchbox', { name: 'Search tournaments' });
  }

  public async NavigateToTournamentListPage(): Promise<void> {
    await this.page.goto(TournamentListPage.TOURNAMENT_LIST_URL);
    await expect(this.page).toHaveTitle(TournamentListPage.PAGE_TITLE);
  }

  public async VerifyTitleIsDisplayed(): Promise<void> {
    await expect(this.title).toHaveText(TournamentListPage.TITLE_TEXT);
    await expect(this.title).toBeVisible();
  }

  public async VerifyTableIsDisplayed(): Promise<void> {
    await expect(this.table).toBeVisible();
  }

  public async GetTableHeaderTitles(): Promise<string[]> {
    const count = await this.tableHeaders.count();
    const titles: string[] = [];
    for (let i = 0; i < count; i++) {
      titles.push(await this.tableHeaders.nth(i).innerText());
    }
    return titles;
  }

  public async GetTournamentsWithInactiveIndicator(): Promise<string[]> {
    const rows = this.tableRows;
    const count = await rows.count();
    const titles: string[] = [];
    for (let i = 0; i < count; i++) {
      const row = rows.nth(i);
      const indicator = row.locator('span:has(i.fa-ban)');
      if (await indicator.count() > 0) {
        const title = await row.locator('th[scope="row"] .tournament-title').innerText();
        titles.push(title);
      }
    }
    return titles;
  }

  public async GetTournamentCount(): Promise<number> {
    return await this.tableRows.count();
  }

  public async GetTournamentTitles(): Promise<string[]> {
    return await this.table.locator('tbody th[scope="row"]').allInnerTexts();
  }

  public async ToggleShowInactiveOption(state: boolean): Promise<void> {
    await this.showInactiveOption.setChecked(state);
  }

  public async GetShowInactiveOption(): Promise<boolean> {
    return await this.showInactiveOption.isChecked();
  }

  public async GetSearchBoxValue(): Promise<string> {
    return await this.searchBox.inputValue();
  }

  public async SetSearchBoxValue(value: string): Promise<void> {
    await this.searchBox.fill(value);
    await this.searchBox.press('Enter');
  }

  public async ClearSearchBox(): Promise<void> {
    await this.searchBox.clear();
    await this.searchBox.press('Enter');
  }
}
